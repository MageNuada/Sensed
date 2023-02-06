using Avalonia.Controls;
using ReactiveUI.Fody.Helpers;

namespace Sensed.ViewModels;

public class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
    }

    public void SetMainViewCommand()
    {
        if (Design.IsDesignMode)
            return;

        ActiveViewModel = new PeopleViewModel();
    }

    public string Greeting => "Welcome to Avalonia!";

    [Reactive]
    public ViewModelBase ActiveViewModel { get; set; }
}
