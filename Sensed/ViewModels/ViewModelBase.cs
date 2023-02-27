using Avalonia.Controls;
using ReactiveUI;
using Sensed.Data;
using System.Threading.Tasks;

namespace Sensed.ViewModels;

public interface IViewControlled
{
    public ViewController ViewController { get; }
}

public class ViewModelBase : ReactiveObject, IViewControlled
{
    private static int _counter = 0;
    private bool _inited;
    private int _id;

    public ViewModelBase(ViewController viewController)
    {
        _id = _counter++;
        ViewController = viewController;
    }

    public Task Init()
    {
        if (Design.IsDesignMode) return Task.CompletedTask;
        if (_inited) return Task.CompletedTask;
        _inited = true;
        return OnInit();
    }

    public Task Activate() { return OnActivation(); }

    public void Deactivate() { OnDeactivate(); }

    public void Close() { OnClose(); }

    /// <summary>
    /// Вызывается при каждом открытии вью, к которой прикреплена данная вьюмодел
    /// </summary>
    /// <returns></returns>
    protected virtual Task OnActivation() { return Task.FromResult(0); }

    /// <summary>
    /// Вызывается при первой загрузке вью, к которой прикреплена данная вьюмодел
    /// </summary>
    /// <returns></returns>
    protected virtual Task OnInit() { return Task.FromResult(0); }

    /// <summary>
    /// Вызывается при закрытии (смене на другое) вью, к которой прикреплена данная вьюмодел
    /// </summary>
    protected virtual void OnDeactivate() { }

    protected virtual void OnClose() { }

    public override string ToString()
    {
        return $"{base.ToString()} id: {_id}";
    }

    public ViewController ViewController { get; }
}

public class ConnectedViewModelBase : ViewModelBase
{
    public ConnectedViewModelBase(IDataProvider dataProvider, ViewController viewController) : base(viewController)
    {
        DataProvider = dataProvider;
    }

    public IDataProvider DataProvider { get; }
}