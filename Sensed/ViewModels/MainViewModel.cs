using Avalonia.Controls;
using Avalonia.Platform.Storage;
using ReactiveUI.Fody.Helpers;
using Sensed.Data;
using Sensed.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sensed.ViewModels;

public class MainViewModel : ViewModelBase
{
    public MainViewModel() : base(null)
    {
    }

    public MainViewModel(ViewController viewController) : base(viewController)
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

            var resultTask = Task.CompletedTask;
            if (string.IsNullOrEmpty(CurrentId))
            {
                //Света, алло, тут вызов передача твоей вьюмодели контрола регистрации
            }
            else
            {
                var accs = await MainDataProvider.GetAccounts(new[] { CurrentId });
                CurrentProfile = new Account(accs.FirstOrDefault(), MainDataProvider);
                ViewController.OpenView(new PeopleViewModel(MainDataProvider, ViewController));
            }

            return resultTask.ContinueWith(x => base.OnInit());
        });
    }

    #endregion

    public void SelectTabCommand(object o)
    {
        if (Design.IsDesignMode) return;

        if (!int.TryParse(o as string, out var index)) return;

        ViewModelBase vm = index switch
        {
            0 => new PeopleViewModel(MainDataProvider, ViewController),
            1 => new ChatsViewModel(MainDataProvider, ViewController),
            2 => new LikedAccountsViewModel(MainDataProvider, ViewController),
            3 => new AccountViewModel(MainDataProvider, CurrentProfile, ViewController),
            _ => throw new ArgumentException("Wrong tab selected!"),
        };

        //if (!OpenedViewModels.Any() || OpenedViewModels.Last() != ActiveViewModel)
        //{
        //    OpenedViewModels.Add(ActiveViewModel);
        //}

        ViewController.OpenView(vm);
    }

    /// <summary>
    /// Активная в данный момент вьюмодель для приложения
    /// </summary>
    [Reactive] internal ViewModelBase? ActiveViewModel { get; set; }

    internal IStorageProvider? StorageProvider { get; set; }

    private IDataProvider? MainDataProvider { get; set; }

    public string? CurrentId { get; private set; }

    public (string parameter, InfoType type)[] VariationsList { get; private set; } = Array.Empty<(string, InfoType)>();

    public Account CurrentProfile { get; private set; }
}
