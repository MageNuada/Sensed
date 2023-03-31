using Avalonia.Controls;
using ReactiveUI.Fody.Helpers;
using Sensed.Models;

namespace Sensed.ViewModels
{
	public class LogInViewModel : ControlledViewModelBase
    {
        public LogInViewModel() : base() { }

        public LogInViewModel(ViewController viewController) : base(viewController)
        {

        }

        public void OpenRegistrationCommand()
        {
            if (Design.IsDesignMode) return;

            ViewController.OpenView<RegistrationViewModel>(true);
        }
    }
}