using Sensed.Data;

namespace Sensed.ViewModels;

public class SettingsViewModel : ConnectedViewModelBase
{
    public SettingsViewModel() : base(null, null) { }

    public SettingsViewModel(IDataProvider dataProvider, ViewController viewController) : base(dataProvider, viewController)
    {
    }
}
