using Avalonia.Controls;
using Avalonia.Platform.Storage;
using ReactiveUI.Fody.Helpers;
using Sensed.Data;
using Sensed.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sensed.ViewModels;

public class MainViewModel : ViewModelBase, IViewControlled
{
    public MainViewModel() : base()
    {
    }

    public MainViewModel(ViewController viewController) : base()
    {
        ViewController = viewController;
    }

    #region Overriden

    protected override Task OnInit()
    {
        return Task.Run(async () =>
        {
            var idTask = ViewController.DataProvider.Login("+79991234567");
            var paramsTask = ViewController.DataProvider.GetSGParams();
            CurrentId = await idTask;
            VariationsList = (await paramsTask).ToArray();

            //CurrentId = null;
            if (string.IsNullOrEmpty(CurrentId))
            {
                ViewController.OpenView<RegistrationViewModel>();
            }
            else
            {
                var accs = await ViewController.DataProvider.GetAccounts(new[] { CurrentId });
                CurrentProfile = new Account(accs.FirstOrDefault(), ViewController.DataProvider);
                ViewController.OpenView<PeopleViewModel>();
            }

            return Task.CompletedTask.ContinueWith(x => base.OnInit());
        });
    }

    #endregion

    public void SelectTabCommand(object o)
    {
        if (Design.IsDesignMode) return;

        if (!int.TryParse(o as string, out var index)) return;

        ViewModelBase vm = index switch
        {
            0 => ViewController.GetOrCreateView<PeopleViewModel>(),
            1 => ViewController.GetOrCreateView<ChatsViewModel>(),
            2 => ViewController.GetOrCreateView<LikedAccountsViewModel>(),
            3 => ViewController.GetOrCreateView<AccountViewModel>(CurrentProfile, ViewController),
            _ => throw new ArgumentException("Wrong tab selected!"),
        };

        ViewController.OpenView(vm);
    }

    internal void SetViewModel(ViewModelBase? viewModel)
    {
        ActiveViewModel = viewModel;
    }

    /// <summary>
    /// Активная в данный момент вьюмодель для приложения
    /// </summary>
    [Reactive] internal ViewModelBase? ActiveViewModel { get; private set; }

    internal IStorageProvider? StorageProvider { get; set; }

    public string? CurrentId { get; private set; }

    public (string parameter, InfoType type)[] VariationsList { get; private set; } = Array.Empty<(string, InfoType)>();

    public Account CurrentProfile { get; private set; }

    public ViewController ViewController { get; }
}
