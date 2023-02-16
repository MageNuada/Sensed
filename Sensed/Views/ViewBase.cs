using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Sensed.ViewModels;

namespace Sensed.Views;

public class ViewBase<T> : ReactiveUserControl<T> where T : ViewModelBase
{
    public ViewBase()
    {
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (Design.IsDesignMode) return;

        ViewModel?.Activate();
    }

    protected override void OnLoaded()
    {
        base.OnLoaded();

        if (Design.IsDesignMode) return;

        ViewModel?.Init();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        ViewModel?.Deactivate();

        if (Design.IsDesignMode) return;

        base.OnDetachedFromVisualTree(e);
    }
}
