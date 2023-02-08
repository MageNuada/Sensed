using System;

namespace Sensed.Models;

public enum AccountMark
{
    Like,
    Dislike,
    Banned,
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

public class SearchFilter
{
}

public class Account
{
    public Account() { }

    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string[] PhotosIds { get; } = Array.Empty<string>();
}
