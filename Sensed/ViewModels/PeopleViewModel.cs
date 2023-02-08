using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Collections;
using ReactiveUI.Fody.Helpers;
using Avalonia.Controls;
using ReactiveUI;

namespace Sensed.ViewModels
{
    public class PeopleViewModel : ConnectedViewModelBase
    {
        //private bool _imagesChanging;

        public PeopleViewModel(IDataProvider dataProvider) : base(dataProvider)
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>() ?? throw new Exception();
            var bitmap1 = new Bitmap(assets.Open(new Uri("avares://Sensed/Assets/unnamed1.png")));
            var bitmap2 = new Bitmap(assets.Open(new Uri("avares://Sensed/Assets/unnamed2.png")));
            var bitmap3 = new Bitmap(assets.Open(new Uri("avares://Sensed/Assets/unnamed3.png")));
            Images.Add(bitmap1);
            Images.Add(bitmap2);
            Images.Add(bitmap3);

            if (Design.IsDesignMode) return;

            this.WhenAnyValue(x => x.SelectedElementIndex).Subscribe(x =>
            {
                return;
            });
        }

        public AvaloniaList<Bitmap> Images { get; set; } = new();

        [Reactive]
        public int SelectedElementIndex { get; set; }

        [Reactive]
        public Bitmap? SelectedElement { get; set; }
    }
}
