using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Orpheo.Models
{
    public class Playlist
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }

        // M–M cu Song
        public virtual ICollection<PlaylistSong> PlaylistSongs { get; set; } = new List<PlaylistSong>();

        // 1–M: un playlist poate fi folosit de mai multe SessionRooms
        public virtual ICollection<SessionRoom> SessionRooms { get; set; } = new List<SessionRoom>();
        public bool IsPublic { get; set; } = true;
        public string? ImagePath { get; set; }

    }
}
