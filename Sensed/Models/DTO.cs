using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sensed.Models;

public enum AccountMark
{
    Like,
    Dislike,
    Banned,
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
    public List<string> PhotosIds { get; } = new();
}
