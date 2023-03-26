using ReactiveUI.Fody.Helpers;
using Sensed.Data;
using Sensed.Models;
using System.Collections.Generic;
using System.Linq;

namespace Sensed.ViewModels;

public class ChatViewModel : ControlledViewModelBase
{
    public ChatViewModel() : base()
    {
        var testProvider = new StubDataProvider();
        var accs = testProvider.SearchAccounts(new List<SearchFilter>()).Result;
        Account = new Account(accs?.FirstOrDefault(), testProvider);
        LastMessage = "Some text from chat with wrapping";
    }

    public ChatViewModel(Account account, ViewController viewController) : base(viewController)
    {
        Account = account;
    }

    public Account Account { get; }

    [Reactive] public string LastMessage { get; set; }

    [Reactive] bool HasUnread { get; set; }
}
