using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Orpheo.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<SongTag> SongTags { get; set; }
    }
}
