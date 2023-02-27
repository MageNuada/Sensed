using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Sensed.Data;
using Sensed.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sensed.ViewModels;

public class ProfileImage : ReactiveObject
{
    public ProfileImage(Bitmap? image = null)
    {
        Image = image;
    }

    [Reactive] public Bitmap? Image { get; set; }

    [Reactive] public float Width { get; set; } = 102.4f;

    [Reactive] public float Height { get; set; } = 76.8f;

    public override string ToString()
    {
        return Image?.ToString() ?? " NULL";
    }
}

public class FillProfileViewModel : ConnectedViewModelBase, IQueuedView
{
    public FillProfileViewModel() : base(null, null)
    {
        if (!Design.IsDesignMode) throw new Exception("For design view only!");

        Images.AddRange(Enumerable.Range(0,9).Select(x => new ProfileImage()));
    }

    public FillProfileViewModel(Account owner, IDataProvider dataProvider, IStorageProvider? storageProvider, ViewController viewController)
        : base(dataProvider, viewController)
    {
        Owner = owner ?? throw new ArgumentNullException(nameof(owner), "Profile cannot be null!");
        StorageProvider = storageProvider ?? throw new ArgumentNullException(nameof(storageProvider), "Storage provider cannot be null!");
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

    #endregion

    public void Save()
    {
        if (Design.IsDesignMode || Owner == null) return;

        Owner.Photos = Images.Where(x => x.Image != null).Select(x => new Lazy<Task<Bitmap>>(Task.FromResult(x.Image))).ToList();
        Owner.Description = Description;
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
                var data = await image.Value;
                Images.Add(new ProfileImage(data));
            }
            for (int i = Images.Count; i < 9; i++)
                Images.Add(new ProfileImage());

            Description = Owner.Description;
        });
    }

    #region Commands

    public async Task AddImageCommand(object o)
    {
        if (Design.IsDesignMode || Owner == null || StorageProvider == null || o is not ProfileImage image) return;

        var files = await StorageProvider.OpenFilePickerAsync(
            new FilePickerOpenOptions()
            {
                AllowMultiple = false,
                Title = "Load your photo",
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("image")
                    {
                        Patterns = new[] { "*.png", "*.jpg", "*.jpeg" }
                    }
                }
            });

        if (files == null || files.Count == 0) return;

        var file = files[0];
        var stream = await file.OpenReadAsync();
        var bitmap = Bitmap.DecodeToHeight(stream, 768, BitmapInterpolationMode.HighQuality);

        if (bitmap.PixelSize.AspectRatio > 2 || bitmap.PixelSize.AspectRatio < 0.5) return;

        int currentIndex = Images.IndexOf(image);
        var stub = Images.FirstOrDefault(x => x.Image is null);
        if(stub != null)
        {
            int freeIndex = Images.IndexOf(stub);
            Images[freeIndex] = image;
            Images[currentIndex] = stub;
        }

        image.Image = bitmap;
    }

    public void RemoveImageCommand(object o)
    {
        if (Design.IsDesignMode || Owner == null || o is not ProfileImage image) return;

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

    private IStorageProvider? StorageProvider { get; set; }

    public Account Owner { get; }

    public AvaloniaList<ProfileImage> Images { get; } = new() { ResetBehavior = ResetBehavior.Reset };
    
    [Reactive] public string? Description { get; set; }

    private Size ViewSize { get; set; }

    #endregion
}
