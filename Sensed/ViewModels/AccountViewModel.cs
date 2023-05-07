using Avalonia.Controls;
using ReactiveUI.Fody.Helpers;
using Sensed.Models;

namespace Sensed.ViewModels;

public class AccountViewModel : ControlledViewModelBase
{
    private readonly Account _currentProfile;

    public AccountViewModel() : base()
    {

    }

    public AccountViewModel(Account currentProfile, ViewController viewController)
        : base(viewController)
    {
        Profile = ViewController.CreateView<ProfileViewModel>(currentProfile, viewController, false);
        _currentProfile = currentProfile;
    }

    public void OpenProfileEditCommand()
    {
        if (Design.IsDesignMode) return;

        ViewController.OpenView<FillProfileViewModel>(addPreviousToHistory: true, ViewController, _currentProfile);
    }

    public void OpenSettingsCommand()
    {
        if (Design.IsDesignMode) return;

        ViewController.OpenView<SettingsViewModel>(true);
    }

    [Reactive] public ProfileViewModel Profile { get; set; }
}
