using System;
using Avalonia.Collections;
using ReactiveUI.Fody.Helpers;
using Avalonia.Controls;
using System.Threading.Tasks;
using Sensed.Models;
using System.Linq;
using Sensed.Data;

namespace Sensed.ViewModels;

public class PeopleViewModel : ConnectedViewModelBase
{
    public PeopleViewModel() : base(null)
    {
        if (!Design.IsDesignMode) throw new Exception("For design view only!");
        Accounts.Add(new ProfileViewModel(new Account(new AccountDTO(), null
            /*не нужно, потому что список фото пустой*/
            /*new StubDataProvider()*/)));
    }

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
                return new ProfileViewModel(new Account(x, DataProvider));
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

    #region Commands

    public async Task LikeCommand(object o)
    {
        if(Design.IsDesignMode) return;
        if (o is not ProfileViewModel account) return;

        await MarkAccount(account, AccountMark.Like);
    }

    public async Task DislikeCommand(object o)
    {
        if (Design.IsDesignMode) return;
        if (o is not ProfileViewModel account) return;

        await MarkAccount(account, AccountMark.Dislike);
    }

    public async Task BanCommand(object o)
    {
        if (Design.IsDesignMode) return;
        if (o is not ProfileViewModel account) return;

        await MarkAccount(account, AccountMark.Banned);
    }

    public async Task BrightLikeCommand(object o)
    {
        if (Design.IsDesignMode) return;
        if (o is not ProfileViewModel account) return;

        await MarkAccount(account, AccountMark.BrightLike);
    }

    #endregion

    #region Private Methods

    private async Task MarkAccount(ProfileViewModel account, AccountMark mark)
    {
        if (account == null) return;

        RemoveAccountFromCarousel(account);
        await DataProvider.MarkAccount(account.Account.Id, mark);
    }

    private void RemoveAccountFromCarousel(ProfileViewModel account)
    {
        Accounts.Remove(account);
    }

    #endregion

    public AvaloniaList<ProfileViewModel> Accounts { get; set; } = new();

    [Reactive] public bool ShowLoading { get; set; }
}
