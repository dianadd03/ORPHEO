using System;
using System.ComponentModel.DataAnnotations;

namespace Orpheo.Models
{
    public class Comm
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime CreatedAt { get; set; }

        // user 1 - M comms
        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        // song 1 - M comms
        [Required]
        public int SongId { get; set; }
        public virtual Song Song { get; set; }
    }
}
