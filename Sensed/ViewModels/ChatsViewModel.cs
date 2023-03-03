using Avalonia.Controls;
using System;

namespace Sensed.ViewModels;

public class ChatsViewModel : ControlledViewModelBase
{
    public ChatsViewModel() : base(null)
    {
        if (!Design.IsDesignMode) throw new Exception("For design view only!");
    }

    public ChatsViewModel(ViewController viewController) : base(viewController)
    {
    }
}
