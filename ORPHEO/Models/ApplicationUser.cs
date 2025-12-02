using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Orpheo.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }

        // User 1 - M Playlists
        public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();

        // User 1 - M Songs (posted songs)
        public virtual ICollection<Song> Songs { get; set; } = new List<Song>();

        // User 1 - M Comments
        public virtual ICollection<Comm> Comms { get; set; } = new List<Comm>();

        // User M - M SessionRooms
        public virtual ICollection<SessionRoomUser> SessionRoomUsers { get; set; } = new List<SessionRoomUser>();

    }
}
