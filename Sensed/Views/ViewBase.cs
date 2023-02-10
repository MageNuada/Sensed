using Avalonia.ReactiveUI;
using ReactiveUI;
using Sensed.ViewModels;
using System;

namespace Sensed.Views;

public class ViewBase : ReactiveUserControl<ViewModelBase>
{
    public ViewBase()
    {
        this.WhenAnyValue(x => x.ViewModel).Subscribe(x =>
        {
            if (x == null) return;

            Loaded += x.OnViewLoaded;
        });
    }
}
