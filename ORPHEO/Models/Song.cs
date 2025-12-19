using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Orpheo.Models
{
    public class Song
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Url { get; set; }

        [Required]
        public DateTime DataPublicarii { get; set; }

        [Required]
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }

        public string? Lyrics { get; set; }

        // Many-to-many cu Tag
        public virtual ICollection<SongTag> SongTags { get; set; } = new List<SongTag>();

        // Many-to-many cu Playlist
        public virtual ICollection<PlaylistSong> PlaylistSongs { get; set; } = new List<PlaylistSong>();

        // One-to-many cu Comm
        public virtual ICollection<Comm> Comms { get; set; } = new List<Comm>();


    }
}
