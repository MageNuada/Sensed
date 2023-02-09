using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sensed.Models
{
    public class Account
    {
        private readonly IDataProvider _provider;

        public Account(AccountDTO dto, IDataProvider provider)
        {
            _provider = provider;
            Id = dto.Id;
            Name = dto.Name;
            Description = dto.Description;
            Photos = dto.PhotosIds.Select(x =>
            new Lazy<Task<Bitmap>>(async() => await
            _provider.GetPhoto(x))).ToList();
        }

        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<Lazy<Task<Bitmap>>> Photos { get; set; } = new();
    }
}
