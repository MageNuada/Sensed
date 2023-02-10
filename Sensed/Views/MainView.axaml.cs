using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Sensed.ViewModels;
using System;

namespace Sensed.Views;

public partial class MainView : ReactiveUserControl<MainViewModel>
{
    public MainView()
    {
        InitializeComponent();

        this.WhenAnyValue(x => x.ViewModel).Subscribe(x =>
        {
            if (x == null) return;

            Loaded += x.OnViewLoaded;
        });
    }
}