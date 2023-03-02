using Sensed.ViewModels;

namespace Sensed.Views;

public partial class LikedAccountsView : ViewBase<LikedAccountsViewModel>
{
    public LikedAccountsView()
    {
        InitializeComponent();

        this.WhenViewModelAnyValue(disposable =>
        {
            this.AddHandler(PointerReleasedEvent, (s,e) => {  });
            //this.Events().
        });
    }
}
