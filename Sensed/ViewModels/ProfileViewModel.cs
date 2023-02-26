using Avalonia.Controls;
using ReactiveUI.Fody.Helpers;
using Sensed.Models;
using System;

namespace Sensed.ViewModels;

public class ProfileViewModel : ViewModelBase
{
    public ProfileViewModel() : base(null)
    {
        if (!Design.IsDesignMode) throw new Exception("For design view only!");

        Account = new Account(new AccountDTO(), null);
    }


    public ProfileViewModel(Account account, ViewController viewController) : base(viewController)
    {
        Account = account;
    }

    [Reactive] public Account Account { get; set; }
}
