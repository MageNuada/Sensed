using System;
using Avalonia.Collections;
using ReactiveUI.Fody.Helpers;
using Avalonia.Controls;
using System.Threading.Tasks;
using Sensed.Models;
using System.Linq;
using Sensed.Data;
using System.Collections.Generic;
using System.Collections;

namespace Sensed.ViewModels
{
    public class RegistrationViewModel : ConnectedViewModelBase
    {
        [Reactive] public IEnumerable Genders { get; set; } = Enum.GetValues(typeof(Gender));
        public RegistrationViewModel() : base(null, null)
        {
            //if (!Design.IsDesignMode) throw new Exception("For design view only!");
            //RegistrationScreens.Add(new UserControl());
            //var genders = Enum.GetValues(typeof(Gender));
        }

        public RegistrationViewModel(IDataProvider dataProvider, ViewController viewController) : base(dataProvider, viewController)
        {
            if (Design.IsDesignMode) return;
        }

    }
}
