using Avalonia.Collections;
using Avalonia.Threading;
using Sensed.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Sensed.ViewModels;

public class ChatsListViewModel : ControlledViewModelBase
{
    public ChatsListViewModel() : base()
    {
        MatchedAccounts.Add(new ChatViewModel());
        MatchedAccounts.Add(new ChatViewModel());
        MatchedAccounts.Add(new ChatViewModel());
    }

    public ChatsListViewModel(ViewController viewController) : base(viewController)
    {
    }

    protected override Task OnActivation()
    {
        return Task.Run(async () =>
        {
            var allAccs = await ViewController.DataProvider.GetMatchedAccounts();
            var accs = allAccs.Where(a => a.mark >= AccountMark.Like && a.whos == LikeSource.BothSidesLike)
            .Select(a => ViewController.CreateView<ChatViewModel>(new Account(a.account, ViewController.DataProvider), ViewController));
            Dispatcher.UIThread.Post(() =>
            {
                MatchedAccounts.Clear();
                MatchedAccounts.AddRange(accs);
            });

            return base.OnActivation();
        });
    }

    public AvaloniaList<ChatViewModel> MatchedAccounts { get; } = new() { ResetBehavior = ResetBehavior.Reset };
}
