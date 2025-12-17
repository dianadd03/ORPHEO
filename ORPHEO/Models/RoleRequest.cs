namespace Orpheo.Models
{
    public class RoleRequest
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string RequestedRole { get; set; } = "Artist";
        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
