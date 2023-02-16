using ReactiveUI.Fody.Helpers;
using Sensed.Data;

namespace Sensed.ViewModels;

public class AccountViewModel : ConnectedViewModelBase
{
    public AccountViewModel() : base(null)
    {

    }

    public AccountViewModel(IDataProvider dataProvider, Models.Account currentProfile) : base(dataProvider)
    {
        ViewModel = new ProfileViewModel(currentProfile);
    }

    [Reactive] public ViewModelBase ViewModel { get; set; }
}
