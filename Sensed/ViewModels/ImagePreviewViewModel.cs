using Avalonia.Controls;
using Avalonia.Media.Imaging;

namespace Sensed.ViewModels;

public class ImagePreviewViewModel : ControlledViewModelBase
{
    public ImagePreviewViewModel() : base() { }

    public ImagePreviewViewModel(Bitmap image, ViewController viewController, bool showReturn = false) : base(viewController)
    {
        Image = image;
        ShowReturn = showReturn;
    }

    public void ShowFullImageCommand()
    {
        if (Design.IsDesignMode) return;

        ViewController.OpenView(this, true);
    }

    public void ReturnCommand()
    {
        if (Design.IsDesignMode) return;

        ViewController.ReturnPrevious();
    }

    public Bitmap? Image { get; }

    public bool ShowReturn { get; }
}
