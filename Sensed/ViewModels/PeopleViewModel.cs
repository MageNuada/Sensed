using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sensed.ViewModels
{
    public class PeopleViewModel : ViewModelBase
    {
        public PeopleViewModel()
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            var bitmap = new Bitmap(assets.Open(new Uri("avares://Sensed/Assets/unnamed.png")));
            for (int i = 0; i < 10; i++)
                Images.Add(bitmap);
        }

        public List<Bitmap> Images { get; set; } = new List<Bitmap>();
    }
}
