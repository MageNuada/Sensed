using Avalonia.Controls;
using ReactiveUI.Fody.Helpers;
using Sensed.Data;
using Sensed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sensed.ViewModels;

public class ProfileViewModel : ControlledViewModelBase, IQueuedView
{
    public ProfileViewModel() : base(null)
    {
        if (!Design.IsDesignMode) throw new Exception("For design view only!");

        var testProvider = new StubDataProvider();
        var accs = testProvider.SearchAccounts(new List<SearchFilter>()).Result;
        Account = new Account(accs?.FirstOrDefault(), testProvider);
    }

    public ProfileViewModel(Account account, ViewController viewController, bool smallView = false) : base(viewController)
    {
        Account = account;
        SmallView = smallView;
    }

    public void SelectProfileCommand()
    {
        if (Design.IsDesignMode) return;

        ViewController.OpenView(this, true);
    }

    public void SelectChatCommand()
    {
        if (Design.IsDesignMode) return;

        //ViewController.OpenView(this, true);
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

        if (await ViewController.DataProvider.MarkAccount(account.Account.Id, mark) == OperationResult.Success)
        {
            GetOnPreviousViewCommand();
        }
    }

    [Reactive] public Account Account { get; set; }

    [Reactive] public bool SmallView { get; set; }
}
