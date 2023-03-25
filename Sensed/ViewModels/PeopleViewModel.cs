using System;
using Avalonia.Collections;
using ReactiveUI.Fody.Helpers;
using Avalonia.Controls;
using System.Threading.Tasks;
using Sensed.Models;
using System.Linq;

namespace Sensed.ViewModels;

public class PeopleViewModel : ControlledViewModelBase
{
    private AvaloniaList<ProfileViewModel>? _accs = null;
    private int _selectedIndex;

    public PeopleViewModel() : base(null)
    {
        if (!Design.IsDesignMode) throw new Exception("For design view only!");
        Accounts.Add(new ProfileViewModel());
    }

    public PeopleViewModel(ViewController viewController) : base(viewController)
    {
        if (Design.IsDesignMode) return;
    }

    protected override Task OnInit()
    {
        return Task.Run(async () =>
        {
            ShowLoading = true;
            var accsDto = await ViewController.DataProvider.SearchAccounts(Array.Empty<SearchFilter>());
            Accounts.AddRange(accsDto.Select(x =>
            {
                return ViewController.CreateView<ProfileViewModel>(new Account(x, ViewController.DataProvider), ViewController, false);
                //это позволяет нам прогрузить первое фото для профиля сразу
                //var acc = new Account(x, DataProvider);
                //var ph = acc.Photos[0].Value.Result;
                //return acc;
            }));

            ShowLoading = false;
            return base.OnInit();
        });
    }

    protected override Task OnActivation()
    {
        if (_accs != null)
        {
            Accounts = _accs;
            SelectedProfileIndex = _selectedIndex;
        }

        return base.OnActivation();
    }

    protected override void OnDeactivate()
    {
        base.OnDeactivate();

        _accs = new(Accounts);
        _selectedIndex = SelectedProfileIndex;
        Accounts.Clear();
    }

    #region Commands

    public async Task LikeCommand(object o)
    {
        if (Design.IsDesignMode) return;
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

        if (await ViewController.DataProvider.MarkAccount(account.Account.Id, mark) == OperationResult.Success)
        {
            RemoveAccountFromCarousel(account);
        }
    }

    private void RemoveAccountFromCarousel(ProfileViewModel account)
    {
        Accounts.Remove(account);
    }

    #endregion

    [Reactive] public AvaloniaList<ProfileViewModel> Accounts { get; set; } = new();

    [Reactive] public bool ShowLoading { get; set; }

    [Reactive] public int SelectedProfileIndex { get; set; }
}
