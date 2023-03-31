using Avalonia.Controls;
using Sensed.Models;
using System.Threading.Tasks;

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
