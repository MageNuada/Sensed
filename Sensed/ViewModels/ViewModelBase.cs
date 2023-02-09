using ReactiveUI;
using System.Threading.Tasks;

namespace Sensed.ViewModels;

public class ViewModelBase : ReactiveObject
{
    public async Task Activate() { await OnActivate(); }

    public void Deactivate() { OnDeactivate(); }

    public void Close() { OnClose(); }

    protected virtual Task OnActivate() { return Task.FromResult(0); }

    protected virtual void OnDeactivate() { }

    protected virtual void OnClose() { }
}

public class ConnectedViewModelBase : ViewModelBase
{
    public ConnectedViewModelBase(IDataProvider dataProvider)
    {
        DataProvider = dataProvider;
    }

    public IDataProvider DataProvider { get; }
}