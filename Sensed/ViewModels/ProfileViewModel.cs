using Avalonia.Controls;
using ReactiveUI.Fody.Helpers;
using Sensed.Data;
using Sensed.Models;
using System;
using System.Threading.Tasks;

namespace Sensed.ViewModels;

public class ProfileViewModel : ConnectedViewModelBase, IQueuedView
{
    public ProfileViewModel() : base(null, null)
    {
        if (!Design.IsDesignMode) throw new Exception("For design view only!");

        Account = new Account(new AccountDTO(), null);
    }

    public ProfileViewModel(Account account, IDataProvider dataProvider, ViewController viewController, bool smallView = false) : base(dataProvider, viewController)
    {
        Account = account;
        SmallView = smallView;
    }

    public void SelectProfileCommand()
    {
        if (Design.IsDesignMode) return;

        ViewController.OpenView(this, true);
    }

    public async Task LikeCommand(object o)
    {
        if (Design.IsDesignMode) return;
        if (o is not ProfileViewModel account) return;

        await MarkAccount(account, AccountMark.Like);
    }

    public async Task DislikeCommand(object o)
    {
        if (Design.IsDesignMode) return;
        if (o is not ProfileViewModel account) return;

        await MarkAccount(account, AccountMark.Banned);
    }

    public void GetOnPreviousViewCommand()
    {
        ((IQueuedView)this).GetOnPreviousView();
    }
    
    private async Task MarkAccount(ProfileViewModel account, AccountMark mark)
    {
        if (account == null) return;

        await DataProvider.MarkAccount(account.Account.Id, mark);

        GetOnPreviousViewCommand();
    }

    [Reactive] public Account Account { get; set; }

    [Reactive] public bool SmallView { get; set; }
}
