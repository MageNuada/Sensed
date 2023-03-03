using System;

namespace Sensed.Models;

public enum Gender
{
    Man,
    Woman,
    Other,
    Agender,
    Androgynous,
    Bigender,
    GenderFluid,
    GenderNonComforming,
    Genderqueer,
    GenderQuestioning,
    Intersex,
    NonBinary,
    Pangender,
    TransHuman,
    TransMan,
    TransWoman,
    Transfemenine,
    Transmasculine,
    TwoSpirit
}

[Flags]
public enum AccountMark
{
    Banned = 1,
    Dislike = 2,
    Like = 4,
    BrightLike = 8,
}

public enum OperationResult
{
    Success,
    Fail,
    Undefined,
}

public enum AccountStatus
{
    Standard,
    Level1,
    Level2,
    Level3,
}

public enum InfoType
{
    Gender,
    Orientation,
    Desire,
}

public class SearchFilter
{
}

public interface IAccount
{
    string Id { get; set; }
    string? Name { get; set; }
    string? Description { get; set; }
    DateTime Birthday { get; set; }
    string? SexualOrientation { get; set; }
    string? GenderIdentity{ get; set; }
    string? RelationsStatus { get; set; }
    string? LastSeen { get; set; }
    string? Position { get; set; }
}

public class AccountDTO : IAccount
{
    public AccountDTO() { }

    public string Id { get; set; }
    public string? Name { get; set; }
    public DateTime Birthday { get; set; }
    public string? Description { get; set; }
    public string? SexualOrientation { get; set; }
    public string? GenderIdentity { get; set; }
    public string? RelationsStatus { get; set; }
    public string? LastSeen { get; set; }
    public string? Position { get; set; }
    public string[] Interests { get; set; } = Array.Empty<string>();
    public string[] Desires { get; set; } = Array.Empty<string>();
    public string[] PhotosIds { get; set; } = Array.Empty<string>();
}