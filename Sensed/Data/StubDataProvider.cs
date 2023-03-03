using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia;
using Sensed.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sensed.Data;

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
                GenderIdentity = "Man",
                Position = "Moscow, 5 km from you",
                SexualOrientation = "Straight",
                LastSeen = "5 minutes ago",
                RelationsStatus = "Single",
                PhotosIds = new[] { "1", "2", "3" }
            },
            new AccountDTO
            {
                Id = Guid.NewGuid().ToString(),
                Name="Test2",
                Description="i am newbie here",
                GenderIdentity = "Woman",
                Position = "Moscow, 1 km from you",
                SexualOrientation = "Straight",
                LastSeen = "last seen",
                RelationsStatus = "Single",
                PhotosIds = new[] { "4", "5", "6" }
            },
            new AccountDTO
            {
                Id = Guid.NewGuid().ToString(),
                Name="Test3",
                Description="0123456789",
                GenderIdentity = "Woman",
                Position = "Moscow, 15 km from you",
                SexualOrientation = "Bisexual",
                LastSeen = "last seen",
                RelationsStatus = "Couple",
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
        return Task.Delay(150).ContinueWith(x => _profiles as IEnumerable<AccountDTO>);
    }

    public Task<AccountStatus> GetAccountStatus()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<(AccountDTO account, AccountMark mark, int whos)>> GetMatchedAccounts()
    {
        return Task.Delay(50).ContinueWith(x => 
        new[]
        {
            (_profiles[0], AccountMark.Like, 1),
            (_profiles[1], AccountMark.Like, 1),
            (_profiles[2], AccountMark.Like, 1),
        }
        as IEnumerable<(AccountDTO account, AccountMark mark, int whos)>);
    }

    public Task<Bitmap> GetPhoto(string photoId)
    {
        return Task.Delay(50).ContinueWith(x => _images[photoId]);
    }

    public Task<string> UploadPhoto(Bitmap photo)
    {
        return Task.Delay(1500).ContinueWith(x => "12345");
    }

    public Task<OperationResult> DeletePhoto(string photoId)
    {
        return Task.Delay(500).ContinueWith(x => OperationResult.Success);
    }

    public Task<string?> Login(string phone)
    {
        return Task.Delay(50).ContinueWith<string?>(x => { CheckNumber(phone); return CurrentId = _profiles[0].Id; });
    }

    public Task<OperationResult> MarkAccount(string id, AccountMark mark, string? description = null)
    {
        return Task.Delay(50).ContinueWith(x => OperationResult.Success);
    }

    public Task<OperationResult> ModifyAccount(AccountDTO account)
    {
        return Task.Delay(50).ContinueWith(x => OperationResult.Success);
    }

    public Task<IEnumerable<AccountDTO>> SearchAccounts(IEnumerable<SearchFilter> filters)
    {
        return Task.Delay(150).ContinueWith(x => _profiles as IEnumerable<AccountDTO>);
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

    public Task<IEnumerable<(string parameter, InfoType type)>> GetSGParams()
    {
        return Task.Delay(100).ContinueWith(x =>
        {
            return new[]
            {
                ("Man", InfoType.Gender),
                ("Woman", InfoType.Gender),
                ("Straight", InfoType.Orientation),
                ("bisexual", InfoType.Orientation),
                ("Kink", InfoType.Desire),
                ("Casual", InfoType.Desire),
            } as IEnumerable<(string parameter, InfoType type)>;
        });
    }
}
