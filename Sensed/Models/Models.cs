using Avalonia.Media.Imaging;
using Sensed.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sensed.Models
{
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
            Photos = dto.PhotosIds.Select(x =>
            new Lazy<Task<Bitmap>>(async () => await
            _provider.GetPhoto(x))).ToList();
            Interests.AddRange(dto.Interests);
            Desires.AddRange(dto.Desires);
        }

        public string? Id { get; set; }
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
        public List<Lazy<Task<Bitmap>>> Photos { get; set; } = new();

        public string Summary => $"{DateTime.Now.Year - Birthday.Year} {GenderIdentity} " +
            $"{SexualOrientation} {RelationsStatus} {Position} {LastSeen}";
    }
}
