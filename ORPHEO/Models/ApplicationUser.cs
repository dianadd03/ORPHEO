using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Orpheo.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        // User 1 - M Playlists
        public virtual ICollection<Playlist>? Playlists { get; set; }

        // User 1 - M Songs (posted songs)
        public virtual ICollection<Song>? Songs { get; set; }

        // User 1 - M Comments
        public virtual ICollection<Comm>? Comms { get; set; }

        // User M - M SessionRooms
        public virtual ICollection<SessionRoomUser>? SessionRoomUsers { get; set; }

    }
}
