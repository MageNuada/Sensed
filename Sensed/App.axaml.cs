using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Sensed.ViewModels;
using Sensed.Views;

namespace Sensed;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var controller = new ViewController();
        MainViewModel? viewModel = new(controller);
        controller.SetMainView(viewModel);
        controller.OpenView(new LoadingViewModel());

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow() { DataContext = viewModel };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView() { DataContext = viewModel };
        }

        base.OnFrameworkInitializationCompleted();
    }
}