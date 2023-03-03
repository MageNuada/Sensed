using Avalonia.Controls;
using Sensed.ViewModels;

namespace Sensed.Views;

public partial class RegistrationView : ViewBase<RegistrationViewModel>
{
    private Carousel _carousel;
    private Button _right;
    private Button _left;
    

    public RegistrationView()
    {
        InitializeComponent();

        _carousel = this.Get<Carousel>("carousel");
        _right = this.Get<Button>("right");
        _right.Click += (s, e) => _carousel.Next();
        _left=this.Get<Button>("left");
        _left.Click += (s, e) => _carousel.Previous();
        
    }

}

