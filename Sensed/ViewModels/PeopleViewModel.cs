using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia;
using System;
using Avalonia.Collections;
using ReactiveUI.Fody.Helpers;
using Avalonia.Controls;
using ReactiveUI;
using System.Threading.Tasks;
using Sensed.Models;
using System.Linq;

namespace Sensed.ViewModels
{
    public class PeopleViewModel : ConnectedViewModelBase
    {
        //private bool _imagesChanging;

        public PeopleViewModel() : base(null) { if (!Design.IsDesignMode) throw new Exception("For design view only!"); }

        public PeopleViewModel(IDataProvider dataProvider) : base(dataProvider)
        {
            if (Design.IsDesignMode) return;
        }

        protected async override Task OnActivate()
        {
            var accsDto = await DataProvider.SearchAccounts(Array.Empty<SearchFilter>());
            Accounts.AddRange(accsDto.Select(x =>
            {
                //это позволяет нам прогрузить первое фото для профиля сразу
                var acc = new Account(x, DataProvider);
                var ph = acc.Photos[0].Value.Result;
                return acc;
            }));

            await base.OnActivate();
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();

            Accounts.Clear();
        }

        public AvaloniaList<Account> Accounts { get; set; } = new();
    }
}
