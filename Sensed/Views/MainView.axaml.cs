using Avalonia.Controls;
using Avalonia;
using Sensed.ViewModels;
using Avalonia.VisualTree;

namespace Sensed.Views;

public partial class MainView : ViewBase<MainViewModel>
{
    public MainView()
    {
        InitializeComponent();
        this.WhenViewModelAnyValue(x =>
        {
            if (ViewModel != null)
                ViewModel.StorageProvider = ((this as Visual)?.GetVisualRoot() as TopLevel)?.StorageProvider;
        });
    }

    public bool OnBackButton(int keyCode, object? e)
    {
        if (ViewModel == null) return false;

        if(ViewModel.ViewController.ReturnPrevious()) return true;

        //if not handled
        return false;

        //if handled
        return true;
    }
}