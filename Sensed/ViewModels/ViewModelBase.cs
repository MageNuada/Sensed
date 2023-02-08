using ReactiveUI;

namespace Sensed.ViewModels;

public class ViewModelBase : ReactiveObject
{
}

public class ConnectedViewModelBase : ViewModelBase
{
    public ConnectedViewModelBase(IDataProvider dataProvider)
    {
        DataProvider = dataProvider;
    }

    public IDataProvider DataProvider { get; }
}