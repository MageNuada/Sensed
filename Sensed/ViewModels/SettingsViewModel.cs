using Avalonia.Controls;

namespace Sensed.ViewModels;

public class SettingsViewModel : ControlledViewModelBase
{
    public SettingsViewModel() : base() { }

    public SettingsViewModel(ViewController viewController) : base(viewController)
    {

    }

    public void LogOutCommand()
    {
        if (Design.IsDesignMode) return;

        ViewController.OpenView<LogInViewModel>(true);
    }
}
