using Avalonia.Collections;
using Sensed.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Sensed.ViewModels;

public class ChatsListViewModel : ControlledViewModelBase
{
    public ChatsListViewModel() : base()
    {
        MatchedAccounts.Add(new ProfileViewModel());
        MatchedAccounts.Add(new ProfileViewModel());
        MatchedAccounts.Add(new ProfileViewModel());
    }

    public ChatsListViewModel(ViewController viewController) : base(viewController)
    {
    }

    protected override Task OnActivation()
    {
        return Task.Run(async () =>
        {
            var allAccs = await ViewController.DataProvider.GetMatchedAccounts();
            var accs = allAccs.Where(a => a.mark >= AccountMark.Like && a.whos == 2)
            .Select(a => ViewController.CreateView<ProfileViewModel>(new Account(a.account, ViewController.DataProvider), ViewController, true));
            MatchedAccounts.AddRange(accs);

            return base.OnActivation();
        });
    }

    public AvaloniaList<ProfileViewModel> MatchedAccounts { get; } = new();
}
