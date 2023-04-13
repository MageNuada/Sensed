using System;
using ReactiveUI.Fody.Helpers;
using Avalonia.Controls;
using Sensed.Models;
using System.Collections;
using Avalonia.Collections;
using DynamicData;

namespace Sensed.ViewModels
{
    public class RegistrationViewModel : ControlledViewModelBase
    {
        //будет ли свайпаться карусель регистрации????
        [Reactive] public int RegistrationStep { get; set; } = 1;
        [Reactive] public IEnumerable Genders { get; set; } = Enum.GetValues(typeof(Gender));
        [Reactive] public AvaloniaList<Country> Countries { get; set; }
        [Reactive] public Country SelectedCountry { get; set; }
        public RegistrationViewModel() : base()
        {
            Countries = new AvaloniaList<Country>()
            {
            new Country("Россия", "+7"),
            new Country("Афганистан", "+93"),
            new Country("Армения", "+374")
            };
        }

        public RegistrationViewModel(ViewController viewController) : base(viewController)
        {
            Countries = new AvaloniaList<Country>(ViewController.DataProvider.GetCountries().Result);
        }

        public void NextRegistrationStepCommand(object parameter)
        {
            //if (Design.IsDesignMode) return;

            var a = (Carousel)parameter;
            a.Next();
            if (RegistrationStep < a.ItemCount)
                RegistrationStep++;

        }
        public void PreviousRegistrationStepCommand(object parameter)
        {
            //if (Design.IsDesignMode) return;

            var a = (Carousel)parameter;
            a.Previous();
            if (RegistrationStep > 1)
                RegistrationStep--;
        }
    }
}
