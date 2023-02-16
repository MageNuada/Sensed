using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Sensed.Data;
using Sensed.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        this.WhenAnyValue(x => x.ActiveViewModel).Subscribe(x =>
        {
            if (x != null && !ExistedViewModels.Exists(y => y.GetType() == x.GetType()))
            {
                ExistedViewModels.Add(x);
            }
        });

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

    public bool GetOnPreviousView()
    {
        if (Design.IsDesignMode) return false;

        if (OpenedViewModels.Any())
        {
            var vm = OpenedViewModels[^1];
            OpenedViewModels.RemoveAt(OpenedViewModels.Count - 1);
            Dispatcher.UIThread.Post(() => ActiveViewModel = vm);
            return true;
        }
        else if(ActiveViewModel?.GetType() != typeof(PeopleViewModel))
        {
            var vm = ExistedViewModels.Find(x => x.GetType() == typeof(PeopleViewModel));
            Dispatcher.UIThread.Post(() => ActiveViewModel = vm);
            return true;
        }

        return false;
    }

    public void OpenProfileEditCommand()
    {
        if (Design.IsDesignMode) return;

        OpenedViewModels.Add(ActiveViewModel);
        Dispatcher.UIThread.Post(() => ActiveViewModel = new FillProfileViewModel(CurrentProfile, MainDataProvider, StorageProvider));
    }

    public void OpenSettingsCommand()
    {
        if (Design.IsDesignMode) return;

        OpenedViewModels.Add(ActiveViewModel);
        Dispatcher.UIThread.Post(() => ActiveViewModel = new SettingsViewModel());
    }

    public void SelectTabCommand(object o)
    {
        if (Design.IsDesignMode) return;

        if (!int.TryParse(o as string, out var index)) return;

        ViewModelBase vm = index switch
        {
            0 => new PeopleViewModel(MainDataProvider),
            1 => new ChatsViewModel(MainDataProvider),
            2 => new LikedAccountsViewModel(MainDataProvider),
            3 => new AccountViewModel(MainDataProvider, CurrentProfile),
            _ => throw new ArgumentException("Wrong tab selected!"),
        };

        vm = ExistedViewModels.FirstOrDefault(x => x.GetType() == vm.GetType()) ?? vm;

        //if (!OpenedViewModels.Any() || OpenedViewModels.Last() != ActiveViewModel)
        //{
        //    OpenedViewModels.Add(ActiveViewModel);
        //}

        Dispatcher.UIThread.Post(() => ActiveViewModel = vm);
    }

    private List<ViewModelBase> ExistedViewModels { get; } = new();

    private List<ViewModelBase?> OpenedViewModels { get; } = new();

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
