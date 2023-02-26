using Avalonia.Collections;
using Avalonia.Controls;
using ReactiveUI.Fody.Helpers;
using Sensed.Data;
using Sensed.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sensed.ViewModels;

public class LikedAccountsViewModel : ConnectedViewModelBase
{
    public LikedAccountsViewModel() : base(null, null)
    {
        if (!Design.IsDesignMode) throw new Exception("For design view only!");
    }

    public LikedAccountsViewModel(IDataProvider dataProvider, ViewController viewController) : base(dataProvider, viewController)
    {
    }

    #region Overriden

    protected override Task OnActivation()
    {
        return Task.Run(async () =>
        {
            var allAccs = await DataProvider.GetMatchedAccounts();
            //var accs = allAccs.Where(a => a.mark >= AccountMark.Like && a.whos == 1)
            //.Select(a => new ProfileViewModel(new Account(a.account, DataProvider)));
            //Profiles.AddRange(accs);
            var accs = allAccs.Where(a => a.mark >= AccountMark.Like && a.whos == 1)
            .Select(a => new Account(a.account, DataProvider));
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

    [Reactive] public AvaloniaList<ProfileViewModel> Profiles { get; set; } = new();

    [Reactive] public AvaloniaList<Account> Accounts { get; set; } = new();
}
