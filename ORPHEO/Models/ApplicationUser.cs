using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Orpheo.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Numele afișat în aplicație
        public string? Name { get; set; }

        // Descriere profil
        public string? About { get; set; }

        // Path imagine profil
        public string? ProfileImage { get; set; }

        // hash unic
        [Required]
        [StringLength(8)]
        public string UserCode { get; set; } = null!;

        // Relații
        public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
        public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
        public virtual ICollection<Comm> Comms { get; set; } = new List<Comm>();
        public virtual ICollection<SessionRoomUser> SessionRoomUsers { get; set; } = new List<SessionRoomUser>();

    }
}
