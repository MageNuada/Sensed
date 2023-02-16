using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using ReactiveUI.Fody.Helpers;
using Sensed.Data;
using Sensed.Models;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Sensed.ViewModels;

public class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
    }

    #region Overriden

    protected override Task OnInit()
    {
        MainDataProvider = new StubDataProvider();

        return Task.Run(async () =>
        {
            var idTask = MainDataProvider.Login("+79991234567");
            var paramsTask = MainDataProvider.GetSGParams();
            CurrentId = await idTask;
            VariationsList = (await paramsTask).ToArray();

            //await Task.WhenAll(paramsTask, idTask);
            //CurrentId = idTask.Result;
            //VariationsList = paramsTask.Result.ToArray();

            var resultTask = Task.CompletedTask;
            if (string.IsNullOrEmpty(CurrentId))
            {
                //Света, алло, тут вызов передача твоей вьюмодели контрола регистрации
            }
            else
            {
                var accs = await MainDataProvider.GetAccounts(new[] { CurrentId });
                CurrentProfile = new Account(accs.FirstOrDefault(), MainDataProvider);

                Dispatcher.UIThread.Post(() => ActiveViewModel = new PeopleViewModel(MainDataProvider));
            }

            return resultTask.ContinueWith(x => base.OnInit());
        });
    }

    #endregion

    public void GetOnPreviousView()
    {
        if (Design.IsDesignMode) return;

        if (OpenedViewModels.TryPop(out var vm))
            Dispatcher.UIThread.Post(() => ActiveViewModel = vm);
    }

    public void OpenProfileEditCommand()
    {
        if (Design.IsDesignMode) return;

        OpenedViewModels.Push(ActiveViewModel);
        Dispatcher.UIThread.Post(() => ActiveViewModel = new FillProfileViewModel(CurrentProfile, MainDataProvider, StorageProvider));
    }

    public void OpenSettingsCommand()
    {
        if (Design.IsDesignMode) return;
    }

    public void SelectTabCommand(object o)
    {
        if (Design.IsDesignMode) return;

        if (!int.TryParse(o as string, out var index)) return;

        ViewModelBase vm = index switch
        {
            0 => new PeopleViewModel(MainDataProvider),
            1 => new PeopleViewModel(MainDataProvider),
            2 => new PeopleViewModel(MainDataProvider),
            3 => new AccountViewModel(MainDataProvider, CurrentProfile),
            _ => throw new ArgumentException("Wrong tab selected!"),
        };

        OpenedViewModels.Push(ActiveViewModel);
        Dispatcher.UIThread.Post(() => ActiveViewModel = vm);
    }

    private ConcurrentStack<ViewModelBase?> OpenedViewModels { get; } = new();

    /// <summary>
    /// Активная в данный момент вьюмодель для приложения
    /// </summary>
    [Reactive] public ViewModelBase? ActiveViewModel { get; set; }

    internal IStorageProvider? StorageProvider { get; set; }

    private IDataProvider? MainDataProvider { get; set; }

    public string? CurrentId { get; private set; }

    public (string parameter, InfoType type)[] VariationsList { get; private set; } = Array.Empty<(string, InfoType)>();

    public Account CurrentProfile { get; private set; }
}
