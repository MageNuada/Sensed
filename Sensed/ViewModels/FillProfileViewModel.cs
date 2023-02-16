using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Sensed.Data;
using Sensed.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sensed.ViewModels
{
    public class FillProfileViewModel : ConnectedViewModelBase
    {
        public class ProfileImage : ReactiveObject
        {
            public ProfileImage(Bitmap? image = null)
            {
                Image = image;
            }

            [Reactive] public Bitmap? Image { get; set; }

            [Reactive] public int Width { get; set; }
        }

        public FillProfileViewModel() : base(null)
        {
            if (!Design.IsDesignMode) throw new Exception("For design view only!");

            Images.AddRange(Enumerable.Range(0,9).Select(x => new ProfileImage()));
        }

        public FillProfileViewModel(Account owner, IDataProvider dataProvider) : base(dataProvider)
        {
            if(owner == null) throw new ArgumentNullException(nameof(owner), "Profile cannot be null!");
            Owner = owner;
        }

        protected override Task OnActivation()
        {
            return base.OnActivation();
        }

        protected override Task OnInit()
        {
            return Task.WhenAll(Refresh(), base.OnInit());
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
                    Images.AddRange(Enumerable.Range(0, 9).Select(x => new ProfileImage()));
            });
        }

        #region Commands

        public void AddImageCommand()
        {
            if (Design.IsDesignMode || Owner == null) return;

        }

        public void DeleteImageCommand(object o)
        {
            if (Design.IsDesignMode || Owner == null || o is not ProfileImage image) return;

            Images.Remove(image);
            Images.Add(new ProfileImage());
        }

        #endregion

        #region Properties

        public Account Owner { get; }

        public AvaloniaList<ProfileImage> Images { get; } = new() { ResetBehavior = ResetBehavior.Reset };

        #endregion
    }
}
