using System.Text;
using System.Text.Json;

namespace ORPHEO.Services
{
    public class GoogleSongAiTagService : ISongAiTagService
    {

        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        private static readonly string[] AllowedTags =
        {
            "angry", "breakup", "chill", "dark", "energetic",
            "happy", "hope", "love", "melancholic",
            "nostalgic", "sad"
        };

        public GoogleSongAiTagService(IConfiguration config)
        {
            _apiKey = config["GoogleAI:ApiKey"]
                ?? throw new Exception("GoogleAI:ApiKey missing");
            _httpClient = new HttpClient();
        }

        public async Task<List<string>> AnalyzeLyricsAsync(string lyrics)
        {
            var prompt = $@"
You analyze song lyrics and determine the dominant emotional tone.

Choose EXACTLY 3 tags from this list:
{string.Join(", ", AllowedTags)}

Rules:
- choose the MOST dominant emotions, not neutral or safe ones
- aggressive, violent or intense lyrics MUST include angry or dark
- sad or reflective lyrics MUST include sad or melancholic
- energetic music MUST include energetic
- NEVER reuse the same combination if lyrics differ
- lowercase
- respond ONLY with JSON
- format exactly:
{{""tags"":[""tag1"",""tag2"",""tag3""]}}

Lyrics:
{lyrics}
";

            var requestBody = new
            {
                contents = new[]
                {
            new
            {
                parts = new[]
                {
                    new { text = prompt }
                }
            }
        },
                generationConfig = new
                {
                    temperature = 0.6
                }
            };

            var json = JsonSerializer.Serialize(requestBody);

            var response = await _httpClient.PostAsync(
                "https://generativelanguage.googleapis.com/v1/models/gemini-2.5-flash:generateContent?key=" + _apiKey,
                new StringContent(json, Encoding.UTF8, "application/json")
            );

            var body = await response.Content.ReadAsStringAsync();

            // opțional: vezi răspunsul brut
            File.WriteAllText("gemini_raw.json", body);

            using var doc = JsonDocument.Parse(body);

            string? text = null;

            if (doc.RootElement.TryGetProperty("candidates", out var candidates) &&
                candidates.ValueKind == JsonValueKind.Array &&
                candidates.GetArrayLength() > 0)
            {
                var candidate = candidates[0];

                if (candidate.TryGetProperty("content", out var content) &&
                    content.TryGetProperty("parts", out var parts) &&
                    parts.ValueKind == JsonValueKind.Array)
                {
                    foreach (var part in parts.EnumerateArray())
                    {
                        if (part.TryGetProperty("text", out var textElement))
                        {
                            text = textElement.GetString();
                            break;
                        }
                    }
                }
            }

            // fallback DOAR dacă AI nu a răspuns
            if (string.IsNullOrWhiteSpace(text))
            {
                return new List<string> { "melancholic", "love", "hope" };
            }

            // extragem STRICT JSON-ul din text
            var start = text.IndexOf('{');
            var end = text.LastIndexOf('}');

            if (start == -1 || end == -1 || end <= start)
            {
                return new List<string> { "melancholic", "love", "hope" };
            }

            var jsonOnly = text.Substring(start, end - start + 1);

            using var tagsDoc = JsonDocument.Parse(jsonOnly);

            var tags = tagsDoc.RootElement
                .GetProperty("tags")
                .EnumerateArray()
                .Select(t => t.GetString()!)
                .ToList();

            return tags;
        }


    }
}

