using Avalonia.Controls;
using Avalonia.Interactivity;
using ReactiveUI;
using Sensed.Data;
using System.Threading.Tasks;

namespace Sensed.ViewModels;

public class ViewModelBase : ReactiveObject
{
    private bool _inited;


    public Task Init()
    {
        if(Design.IsDesignMode) return Task.CompletedTask;
        if (_inited) return Task.CompletedTask; 
        _inited = true; 
        return OnInit();
    }

    public Task Activate() { return OnActivation(); }

    public void Deactivate() { OnDeactivate(); }

    public void Close() { OnClose(); }

    protected virtual Task OnActivation() { return Task.FromResult(0); }

    protected virtual Task OnInit() { return Task.FromResult(0); }

    protected virtual void OnDeactivate() { }

    protected virtual void OnClose() { }

    internal void OnViewLoaded(object? sender, RoutedEventArgs e)
    {
        Init();
    }
}

public class ConnectedViewModelBase : ViewModelBase
{
    public ConnectedViewModelBase(IDataProvider dataProvider)
    {
        DataProvider = dataProvider;
    }

    public IDataProvider DataProvider { get; }
}