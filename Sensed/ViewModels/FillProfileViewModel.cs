using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Sensed.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sensed.ViewModels;

public class ProfileImage : ReactiveObject
{
    public ProfileImage(Bitmap? image = null, string? id = null)
    {
        Image = image;
        Id = id;
    }

    [Reactive] public Bitmap? Image { get; set; }

    public string? Id { get; set; }

    [Reactive] public float Width { get; set; } = 102.4f;

    [Reactive] public float Height { get; set; } = 76.8f;

    public override string ToString()
    {
        return Image?.ToString() ?? " NULL";
    }
}

public class FillProfileViewModel : ControlledViewModelBase, IQueuedView
{
    public FillProfileViewModel() : base(null)
    {
        if (!Design.IsDesignMode) throw new Exception("For design view only!");

        Images.AddRange(Enumerable.Range(0,9).Select(x => new ProfileImage()));
    }

    public FillProfileViewModel(ViewController viewController, Account owner)
        : base(viewController)
    {
        Owner = owner ?? throw new ArgumentNullException(nameof(owner), "Profile cannot be null!");
    }

    #region Overriden

    protected override Task OnActivation()
    {
        return base.OnActivation();
    }

    protected override Task OnInit()
    {
        return Task.WhenAll(Refresh(), base.OnInit());
    }

    protected override void OnDeactivate()
    {
        Save();

        base.OnDeactivate();
    }

    protected override void OnClose()
    {
        Save();

        base.OnClose();
    }

    #endregion

    public void Save()
    {
        if (Design.IsDesignMode || Owner == null) return;

        Owner.Photos = Images.Where(x => x.Image != null && x.Id != null)
            .Select(x => new DownloadedPhoto(x.Id, new Lazy<Task<Bitmap>>(Task.FromResult<Bitmap>(x.Image))))
            .ToList();
        Owner.Description = Description;

        ViewController.DataProvider.ModifyAccount(Owner);
    }
    
    internal void SetSize(Size finalSize)
    {
        ViewSize = finalSize;
        float w, h;
        w = (float)(finalSize.Width / 3.0 - 20);
        h = (float)(w * 3 / 4);
        foreach (var image in Images)
        {
            image.Width = w;
            image.Height = h;
        }
    }

    private Task Refresh()
    {
        return Task.Run(async () =>
        {
            foreach (var image in Owner.Photos)
            {
                var data = await image.Image.Value;
                Images.Add(new ProfileImage(data, image.Id));
            }
            for (int i = Images.Count; i < 9; i++)
                Images.Add(new ProfileImage());

            Description = Owner.Description;
        });
    }

    #region Commands

    public async Task AddImageCommand(object o)
    {
        if (Design.IsDesignMode || Owner == null || ViewController.StorageProvider == null || o is not ProfileImage image) return;

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
        var stream = await file.OpenReadAsync();
        var bitmap = Bitmap.DecodeToHeight(stream, 768, BitmapInterpolationMode.HighQuality);

        if (bitmap.PixelSize.AspectRatio > 2 || bitmap.PixelSize.AspectRatio < 0.5) return;

        var id = await ViewController.DataProvider.UploadPhoto(bitmap);
        int currentIndex = Images.IndexOf(image);
        var stub = Images.FirstOrDefault(x => x.Image is null);
        if(stub != null)
        {
            int freeIndex = Images.IndexOf(stub);
            Images[freeIndex] = image;
            Images[currentIndex] = stub;
        }

        image.Image = bitmap;
        image.Id = id;
    }

    public async void RemoveImageCommand(object o)
    {
        if (Design.IsDesignMode || Owner == null || o is not ProfileImage image || image.Id == null) return;

        await ViewController.DataProvider.DeletePhoto(image.Id);

        Images.Remove(image);
        float w, h;
        w = (float)(ViewSize.Width / 3.0 - 20);
        h = (float)(w * 3 / 4);
        Images.Add(new ProfileImage() { Width = w, Height = h });
    }

    public void GetOnPreviousViewCommand()
    {
        ((IQueuedView)this).GetOnPreviousView();
    }

    #endregion

    #region Properties

    public Account Owner { get; }

    public AvaloniaList<ProfileImage> Images { get; } = new() { ResetBehavior = ResetBehavior.Reset };
    
    [Reactive] public string? Description { get; set; }

    private Size ViewSize { get; set; }

    #endregion
}
