using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Sensed.ViewModels;
using System;

namespace Sensed.Views
{
    public partial class LoadingView : ReactiveUserControl<LoadingViewModel>
    {
        public LoadingView()
        {
            InitializeComponent();

            this.WhenAnyValue(x => x.ViewModel).Subscribe(x =>
            {
                if (x == null) return;

                Loaded += x.OnViewLoaded;
            });
        }
    }
}
