namespace ORPHEO.Services
{
    public interface ISongAiTagService
    {

        Task<List<string>> AnalyzeLyricsAsync(string lyrics);

    }
}
