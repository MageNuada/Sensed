using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ReactiveUI.Fody.Helpers;
using Sensed.Data;
using Sensed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sensed.ViewModels;

public class ChatMessage
{
    public ChatMessage(object message, int type = 0, bool owned = true)
    {
        Message = message;
        Type = type;
        Owned = owned;
    }

    public object Message { get; }

    public int Type { get; }

    public bool Owned { get; }
}

public class ChatViewModel : ControlledViewModelBase
{
    public ChatViewModel() : base()
    {
        var testProvider = new StubDataProvider();
        var accs = testProvider.SearchAccounts(new List<SearchFilter>()).Result;
        Account = new Account(accs?.FirstOrDefault(), testProvider);
        LastMessage = "Some text from chat with wrapping";
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>() ?? throw new Exception();
        var bitmap1 = new Bitmap(assets.Open(new Uri("avares://Sensed/Assets/unnamed1.png")));
        var bitmap2 = new Bitmap(assets.Open(new Uri("avares://Sensed/Assets/unnamed2.png")));
        Messages.Add(new ChatMessage("Hi there!"));
        Messages.Add(new ChatMessage("Oh hi! :)", owned: false));
        Messages.Add(new ChatMessage(bitmap1, owned: false));
        Messages.Add(new ChatMessage(bitmap2, owned: true));
    }

    public ChatViewModel(Account account, ViewController viewController) : base(viewController)
    {
        Account = account;
    }

    protected override Task OnActivation()
    {
        return Task.Run(() =>
        {
            Messages.Clear();
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>() ?? throw new Exception();
            var bitmap1 = new Bitmap(assets.Open(new Uri("avares://Sensed/Assets/unnamed1.png")));
            var bitmap2 = new Bitmap(assets.Open(new Uri("avares://Sensed/Assets/unnamed2.png")));
            Messages.Add(new ChatMessage($"Hi there, {Account.Name}!"));
            Messages.Add(new ChatMessage($"Oh hi, {ViewController.MainViewModel.CurrentProfile.Name}! :)", owned: false));
            Messages.Add(new ChatMessage(bitmap1, owned: false));
            Messages.Add(new ChatMessage(bitmap2, owned: true));

            return base.OnActivation();
        });
    }

    public void SelectChatCommand()
    {
        if (Design.IsDesignMode) return;

        ViewController.OpenView(this, true);
    }

    public void ReturnCommand()
    {
        if (Design.IsDesignMode) return;

        ViewController.ReturnPrevious();
    }

    public Account Account { get; }

    [Reactive] public string LastMessage { get; set; }

    [Reactive] bool HasUnread { get; set; }

    public AvaloniaList<ChatMessage> Messages { get; } = new();
}
