public class SongVote
{
    public int Id { get; set; }

    public string UserId { get; set; }
    public int SongId { get; set; }
    public bool IsLike { get; set; }
}
