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
        if (Design.IsDesignMode) return;

        ActiveViewModel = new PeopleViewModel(new StubDataProvider());
    }

    public string Greeting => "Welcome to Sensed!";

    [Reactive]
    public ConnectedViewModelBase? ActiveViewModel { get; set; }
}
