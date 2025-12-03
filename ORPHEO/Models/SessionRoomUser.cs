namespace Orpheo.Models
{
    public class SessionRoomUser
    {

        public int SessionRoomId { get; set; }
        public virtual SessionRoom? SessionRoom { get; set; }

        public string   ? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}
