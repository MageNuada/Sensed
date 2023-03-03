using Avalonia.Media.Imaging;
using Sensed.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sensed.Models;

public class DownloadedPhoto
{
    public DownloadedPhoto(string id, Lazy<Task<Bitmap>> image)
    {
        Id = id;
        Image = image;
    }

    public string Id { get; set; }

    public Lazy<Task<Bitmap>> Image { get; set; }
}

public class Account : IAccount
{
    private readonly IDataProvider _provider;

    public Account(AccountDTO dto, IDataProvider provider)
    {
        _provider = provider;
        Id = dto.Id;
        Name = dto.Name;
        Description = dto.Description;
        SexualOrientation = dto.SexualOrientation;
        GenderIdentity = dto.GenderIdentity;
        RelationsStatus = dto.RelationsStatus;
        LastSeen = dto.LastSeen;
        Position = dto.Position;
        Birthday = dto.Birthday;
        Photos = dto.PhotosIds.Select(x => new DownloadedPhoto(x, LazyDownloadPhoto(x))).ToList();
        Interests.AddRange(dto.Interests);
        Desires.AddRange(dto.Desires);
    }

    private Lazy<Task<Bitmap>> LazyDownloadPhoto(string id) 
        => new(async () => await _provider.GetPhoto(id));

    public static implicit operator AccountDTO(Account account)
    {
        return new AccountDTO()
        {
            Id = account.Id,
            Name = account.Name,
            Description = account.Description,
            Birthday = account.Birthday,
            SexualOrientation = account.SexualOrientation,
            GenderIdentity = account.GenderIdentity,
            RelationsStatus = account.RelationsStatus,
            LastSeen = account.LastSeen,
            Position = account.Position,
            Interests = account.Interests.ToArray(),
            Desires = account.Desires.ToArray(),
            PhotosIds = Array.Empty<string>()
        };
    }

    public string Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime Birthday { get; set; }
    public string? SexualOrientation { get; set; }
    public string? GenderIdentity { get; set; }
    public string? RelationsStatus { get; set; }
    public string? LastSeen { get; set; }
    public string? Position { get; set; }
    public List<string> Interests { get; set; } = new();
    public List<string> Desires { get; set; } = new();
    public List<DownloadedPhoto> Photos { get; set; } = new();

    public string Summary => $"{DateTime.Now.Year - Birthday.Year} {GenderIdentity} " +
        $"{SexualOrientation} {RelationsStatus} {Position} {LastSeen}";
}
