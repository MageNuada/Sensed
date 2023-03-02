using Avalonia.Controls;
using ReactiveUI.Fody.Helpers;
using Sensed.Data;

namespace Sensed.ViewModels;

public class AccountViewModel : ConnectedViewModelBase
{
    public AccountViewModel() : base(null, null)
    {

    }

    public AccountViewModel(IDataProvider dataProvider, Models.Account currentProfile, ViewController viewController)
        : base(dataProvider, viewController)
    {
        ViewModel = new ProfileViewModel(currentProfile, DataProvider, ViewController);
    }

    public void OpenProfileEditCommand()
    {
        if (Design.IsDesignMode) return;

        ViewController.OpenView(new FillProfileViewModel(ViewModel.Account, DataProvider, ViewController), true);
    }

    public void OpenSettingsCommand()
    {
        if (Design.IsDesignMode) return;

        ViewController.OpenView(new SettingsViewModel(), true);
    }

    [Reactive] public ProfileViewModel ViewModel { get; set; }
}
