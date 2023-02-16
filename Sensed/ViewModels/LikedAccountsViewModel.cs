using Avalonia.Controls;
using Sensed.Data;
using System;

namespace Sensed.ViewModels;

public class LikedAccountsViewModel : ConnectedViewModelBase
{
    public LikedAccountsViewModel() : base(null)
    {
        if (!Design.IsDesignMode) throw new Exception("For design view only!");
    }

    public LikedAccountsViewModel(IDataProvider dataProvider) : base(dataProvider)
    {
    }
}
