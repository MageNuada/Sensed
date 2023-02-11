using System;
using Avalonia.Collections;
using ReactiveUI.Fody.Helpers;
using Avalonia.Controls;
using System.Threading.Tasks;
using Sensed.Models;
using System.Linq;
using Sensed.Data;

namespace Sensed.ViewModels
{
    public class PeopleViewModel : ConnectedViewModelBase
    {
        public PeopleViewModel() : base(null) { if (!Design.IsDesignMode) throw new Exception("For design view only!"); }

        public PeopleViewModel(IDataProvider dataProvider) : base(dataProvider)
        {
            if (Design.IsDesignMode) return;
        }

        protected override Task OnInit()
        {
            return Task.Run(async () =>
            {
                ShowLoading = true;
                var accsDto = await DataProvider.SearchAccounts(Array.Empty<SearchFilter>());
                Accounts.AddRange(accsDto.Select(x =>
                {
                    return new Account(x, DataProvider);
                    //это позволяет нам прогрузить первое фото для профиля сразу
                    //var acc = new Account(x, DataProvider);
                    //var ph = acc.Photos[0].Value.Result;
                    //return acc;
                }));

                ShowLoading = false;
                return base.OnInit();
            });
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();

            Accounts.Clear();
        }

        public AvaloniaList<Account> Accounts { get; set; } = new();

        [Reactive] public bool ShowLoading { get; set; }
    }
}
