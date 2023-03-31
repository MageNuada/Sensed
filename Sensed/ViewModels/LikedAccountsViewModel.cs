using Avalonia.Collections;
using Avalonia.Threading;
using ReactiveUI.Fody.Helpers;
using Sensed.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Sensed.ViewModels;

public class LikedAccountsViewModel : ControlledViewModelBase
{
    public LikedAccountsViewModel() : base()
    {
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
            var accs = allAccs.Where(a => a.mark >= AccountMark.Like && a.whos == LikeSource.AnotherUsers)
            .Select(a => ViewController.CreateView<ProfileViewModel>(new Account(a.account, ViewController.DataProvider), ViewController, true));
            Dispatcher.UIThread.Post(() => Accounts.AddRange(accs));

            return base.OnActivation();
        });
    }

    protected override void OnDeactivate()
    {
        Accounts.Clear();
        base.OnDeactivate();
    }

    #endregion

    [Reactive] public AvaloniaList<ProfileViewModel> Accounts { get; set; } = new();
}
