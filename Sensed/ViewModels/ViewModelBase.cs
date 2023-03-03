using Avalonia.Controls;
using ReactiveUI;
using System.Threading.Tasks;

namespace Sensed.ViewModels;

public interface IViewControlled
{
    ViewController ViewController { get; }
}

public class ViewModelBase : ReactiveObject
{
    private static int s_counter = 0;
    private bool _inited;
    private int _id;

    public ViewModelBase()
    {
        _id = s_counter++;
    }

    internal Task Init()
    {
        if (Design.IsDesignMode) return Task.CompletedTask;
        if (_inited) return Task.CompletedTask;
        _inited = true;
        return OnInit();
    }

    internal Task Activate() { return OnActivation(); }

    internal void Deactivate() { OnDeactivate(); }

    internal void Close() { OnClose(); }

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

    /// <summary>
    /// Вызывается при закрытии приложения
    /// </summary>
    protected virtual void OnClose() { }

    public override string ToString()
    {
        return $"{base.ToString()} id: {_id}";
    }
}

public class ControlledViewModelBase : ViewModelBase, IViewControlled
{
    public ControlledViewModelBase(ViewController viewController) : base()
    {
        ViewController = viewController;
    }

    public ViewController ViewController { get; }
}