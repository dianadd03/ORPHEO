using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Orpheo.Models
{
    public class SessionRoom
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string HostUserId { get; set; }
        public virtual ApplicationUser HostUser { get; set; }

        // SessionRoom M - Playlist 1 
        [Required]
        public int PlaylistId { get; set; }
        public virtual Playlist Playlist { get; set; }

        // M - M cu User
        public virtual ICollection<SessionRoomUser> SessionRoomUsers { get; set; }
    }
}
