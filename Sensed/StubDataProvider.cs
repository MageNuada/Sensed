using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia;
using Sensed.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sensed;

internal class StubDataProvider : IDataProvider
{
    private readonly List<AccountDTO> _profiles;
    private readonly Dictionary<string, Bitmap> _images = new();

    internal StubDataProvider()
    {
        _profiles = new List<AccountDTO>
        {
            new AccountDTO
            {
                Id = Guid.NewGuid().ToString(),
                Name="Test1",
                Description="i love new",
                PhotosIds = new[] { "1", "2", "3" }
            },
            new AccountDTO
            {
                Id = Guid.NewGuid().ToString(),
                Name="Test2",
                Description="i am newbie here",
                PhotosIds = new[] { "4", "5", "6" }
            },
            new AccountDTO
            {
                Id = Guid.NewGuid().ToString(),
                Name="Test3",
                Description="0123456789",
                PhotosIds = new[] { "7", "8", "9" }
            },
        };

        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>() ?? throw new Exception();
        var bitmap1 = new Bitmap(assets.Open(new Uri("avares://Sensed/Assets/unnamed1.png")));
        var bitmap2 = new Bitmap(assets.Open(new Uri("avares://Sensed/Assets/unnamed2.png")));
        var bitmap3 = new Bitmap(assets.Open(new Uri("avares://Sensed/Assets/unnamed3.png")));
        _images.Add("1", bitmap1);
        _images.Add("2", bitmap2);
        _images.Add("3", bitmap3);
        _images.Add("4", bitmap1);
        _images.Add("5", bitmap2);
        _images.Add("6", bitmap3);
        _images.Add("7", bitmap1);
        _images.Add("8", bitmap2);
        _images.Add("9", bitmap3);
    }

    private string? CurrentId { get; set; }

    #region Private methods

    private void CheckNumber(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) throw new ArgumentNullException(nameof(phone), "Введён пустой номер телефона");
        //if (!Regex.Match(phone[1..^0], @"^(\+[0-9]{9})$").Success) throw new ArgumentException("Введён некорректный номер телефона", nameof(phone));
    }

    #endregion

    public Task<OperationResult> CreateAccount(string phone)
    {
        return Task.Delay(1500).ContinueWith(x => { CheckNumber(phone); return OperationResult.Success; });
    }

    public Task<OperationResult> DeleteAccount()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AccountDTO>> GetAccounts(IEnumerable<string> ids, IEnumerable<SearchFilter>? filters = null)
    {
        throw new NotImplementedException();
    }

    public Task<AccountStatus> GetAccountStatus()
    {
        throw new NotImplementedException();
    }

    public Task<AccountDTO> GetMatchedAccounts()
    {
        throw new NotImplementedException();
    }

    public Task<Bitmap> GetPhoto(string photoId)
    {
        return Task.Delay(150).ContinueWith(x => _images[photoId]);
    }

    public Task<string?> Login(string phone)
    {
        return Task.Delay(500).ContinueWith(x => { CheckNumber(phone); return CurrentId = _profiles[0].Id; });
    }

    public Task<OperationResult> MarkAccount(string id, AccountMark mark, string? description = null)
    {
        throw new NotImplementedException();
    }

    public Task<OperationResult> ModifyAccount(string description, IEnumerable<object> photos, IEnumerable<object> tags)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AccountDTO>> SearchAccounts(IEnumerable<SearchFilter> filters)
    {
        return Task.Delay(500).ContinueWith(x => _profiles as IEnumerable<AccountDTO>);
    }

    public Task<string> SetAccountStatus(AccountStatus status)
    {
        throw new NotImplementedException();
    }

    public Task<object> StartChat(string id)
    {
        throw new NotImplementedException();
    }

    public Task<string?> VerifyAccount(string phone, string smsCode)
    {
        return Task.Delay(1000).ContinueWith(x =>
        {
            CheckNumber(phone);
            if (smsCode == "123456")
                return CurrentId = _profiles[0].Id;
            else
                return null;
        });
    }
}
