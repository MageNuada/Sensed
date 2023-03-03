using Avalonia.Collections;
using Avalonia.Controls;
using ReactiveUI.Fody.Helpers;
using Sensed.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sensed.ViewModels;

public class LikedAccountsViewModel : ControlledViewModelBase
{
    public LikedAccountsViewModel() : base(null)
    {
        if (!Design.IsDesignMode) throw new Exception("For design view only!");
    }

    public LikedAccountsViewModel(ViewController viewController) : base(viewController)
    {
    }

    #region Overriden

    protected override Task OnActivation()
    {
        return Task.Run(async () =>
        {
            var allAccs = await ViewController.DataProvider.GetMatchedAccounts();
            var accs = allAccs.Where(a => a.mark >= AccountMark.Like && a.whos == 1)
            .Select(a => ViewController.CreateView<ProfileViewModel>(new Account(a.account, ViewController.DataProvider), ViewController, true));
            Accounts.AddRange(accs);

            return base.OnActivation();
        });
    }

    protected override void OnDeactivate()
    {
        Accounts.Clear();
        base.OnDeactivate();
    }

    #endregion

    public void SelectProfileCommand(object s, object e)
    {
        if (Design.IsDesignMode) return;

    }

    [Reactive] public AvaloniaList<ProfileViewModel> Accounts { get; set; } = new();
}
