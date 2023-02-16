using Avalonia;
using Sensed.ViewModels;

namespace Sensed.Views;

public partial class FillProfileView : ViewBase<FillProfileViewModel>
{
    public FillProfileView()
    {
        InitializeComponent();
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        ViewModel?.SetSize(finalSize);
        return base.ArrangeOverride(finalSize);
    }
}
