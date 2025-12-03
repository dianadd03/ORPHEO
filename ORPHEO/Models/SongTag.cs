namespace Orpheo.Models
{
    public class SongTag
    {

        public int SongId { get; set; }
        public virtual Song? Song { get; set; }

        public int TagId { get; set; }
        public virtual Tag? Tag { get; set; }
    }
}
