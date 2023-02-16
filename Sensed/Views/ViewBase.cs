using Avalonia;
using Avalonia.ReactiveUI;
using Sensed.ViewModels;

namespace Sensed.Views;

public class ViewBase<T> : ReactiveUserControl<T> where T : ViewModelBase
{
    public ViewBase()
    {
    }

    protected override void OnLoaded()
    {
        base.OnLoaded();

        ViewModel?.Init();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        ViewModel?.Deactivate();

        base.OnDetachedFromVisualTree(e);
    }
}
