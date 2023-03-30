using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
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
        Messages.Add(new ChatMessage(new ImagePreviewViewModel(bitmap1, null, false), 1, owned: false));
        Messages.Add(new ChatMessage(new ImagePreviewViewModel(bitmap2, null, false), 1, owned: true));
    }

    public ChatViewModel(Account account, ViewController viewController) : base(viewController)
    {
        Account = account;
    }

    protected override Task OnActivation()
    {
        return Task.Run(() =>
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>() ?? throw new Exception();
            var bitmap1 = new Bitmap(assets.Open(new Uri("avares://Sensed/Assets/unnamed1.png")));
            var bitmap2 = new Bitmap(assets.Open(new Uri("avares://Sensed/Assets/unnamed2.png")));
            ChatMessage[] chatMessages = new ChatMessage[]
            {
                new ChatMessage($"Hi there, {Account.Name}!"),
                new ChatMessage($"Oh hi, {ViewController.MainViewModel.CurrentProfile.Name}! :)", owned: false),
                new ChatMessage(ViewController.CreateView<ImagePreviewViewModel>(bitmap1, ViewController, false), 1, owned: false),
                new ChatMessage(ViewController.CreateView<ImagePreviewViewModel>(bitmap2, ViewController, false), 1, owned: true),
            };

            Dispatcher.UIThread.Post(() =>
            {
                Messages.Clear();
                Messages.AddRange(chatMessages);
            });

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

    public void SendMessageCommand()
    {
        if (Design.IsDesignMode || string.IsNullOrWhiteSpace(NewMessageText)) return;

        Messages.Add(new ChatMessage(NewMessageText));
        NewMessageText = null;
    }

    public async void UploadContentCommand()
    {
        if (Design.IsDesignMode) return;

        if (Design.IsDesignMode || ViewController.StorageProvider == null) return;

        var files = await ViewController.StorageProvider.OpenFilePickerAsync(
            new FilePickerOpenOptions()
            {
                AllowMultiple = false,
                Title = "Load your photo",
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("image")
                    {
                        Patterns = new[] { "*.bmp", "*.png", "*.jpg", "*.jpeg" }
                    }
                }
            });

        if (files == null || files.Count == 0) return;

        var file = files[0];
        using var stream = await file.OpenReadAsync();

        if (stream.Length > 20_000_000) return;

        var bitmap = Bitmap.DecodeToHeight(stream, 768, BitmapInterpolationMode.HighQuality);

        if (bitmap.PixelSize.AspectRatio > 2 || bitmap.PixelSize.AspectRatio < 0.5) return;

        var id = await ViewController.DataProvider.UploadPhoto(bitmap);

        Messages.Add(new ChatMessage(ViewController.CreateView<ImagePreviewViewModel>(bitmap, ViewController, false), 1, owned: true));
    }

    public Account Account { get; }

    [Reactive] public string LastMessage { get; set; }

    [Reactive] bool HasUnread { get; set; }

    [Reactive] public AvaloniaList<ChatMessage> Messages { get; set; } = new();

    [Reactive] public string? NewMessageText { get; set; }
}
