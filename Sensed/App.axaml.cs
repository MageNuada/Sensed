using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Sensed.ViewModels;
using Sensed.Views;
using System.Threading.Tasks;
using System.Xml;
using System;

namespace Sensed;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow() { DataContext = new LoadingViewModel() };

            //desktop.MainWindow.Loaded += (s, e) => { vm.Activate(); };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new LoadingView();

            //singleViewPlatform.MainView.Loaded += (s, e) => { vm.Activate(); };
        }

        Task.Run(async () =>
        {
            MainViewModel? viewModel = new();

            await viewModel.Activate();

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                //viewModel.Activate().Wait();

                if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    desktop.MainWindow.DataContext = viewModel;

                    //desktop.MainWindow.Loaded += (s, e) => { vm.Activate(); };
                }
                else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
                {
                    singleViewPlatform.MainView = new MainView() { DataContext = viewModel };

                    //singleViewPlatform.MainView.Loaded += (s, e) => { vm.Activate(); };
                }
            });
        });

        base.OnFrameworkInitializationCompleted();
    }
}