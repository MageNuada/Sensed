using Avalonia.ReactiveUI;
using ReactiveUI;
using Sensed.ViewModels;
using System;
using System.Reactive.Linq;

namespace Sensed.Views;

public class ViewBase<T> : ReactiveUserControl<T> where T : ViewModelBase
{
    public ViewBase()
    {
        this.WhenAnyValue(x => x.ViewModel)
            .Buffer(2, 1)
            .Select(b => (Previous: b[0], Current: b[1]))
            .Subscribe(x => 
            {
                if(x.Previous != null)
                    Loaded -= x.Previous.OnViewLoaded;
                if (x.Current != null)
                    Loaded += x.Current.OnViewLoaded;
            });
    }
}
