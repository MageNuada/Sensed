using System;
using ReactiveUI.Fody.Helpers;
using Avalonia.Controls;
using Sensed.Models;
using System.Collections;

namespace Sensed.ViewModels
{
    public class RegistrationViewModel : ControlledViewModelBase
    {
        [Reactive] public IEnumerable Genders { get; set; } = Enum.GetValues(typeof(Gender));
        public RegistrationViewModel() : base(null)
        {
            //if (!Design.IsDesignMode) throw new Exception("For design view only!");
            //RegistrationScreens.Add(new UserControl());
            //var genders = Enum.GetValues(typeof(Gender));
        }

        public RegistrationViewModel(ViewController viewController) : base(viewController)
        {
            if (Design.IsDesignMode) return;
        }

    }
}
