using Avalonia.Controls;
using Sensed.Data;
using System;

namespace Sensed.ViewModels;

public class ChatsViewModel : ConnectedViewModelBase
{
    public ChatsViewModel() : base(null, null)
    {
        if (!Design.IsDesignMode) throw new Exception("For design view only!");
    }

    public ChatsViewModel(IDataProvider dataProvider, ViewController viewController) : base(dataProvider, viewController)
    {
    }
}
