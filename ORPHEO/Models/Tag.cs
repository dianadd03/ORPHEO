using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Orpheo.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(15, ErrorMessage = "Tag name cannot exceed 15 characters.")]
        public string? Name { get; set; }

        public virtual ICollection<SongTag> SongTags { get; set; } = new List<SongTag>();
    }
}
