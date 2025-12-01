using SharpToken;
using SubtitlesTranslator.Application.Interfaces;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class ChatGptService : IChatGptService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _apiModel;

    public ChatGptService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _apiKey = config["OpenAI:ApiKey"];
        _apiModel = config["OpenAI:ApiModel"];
    }

    public async Task<string> TranslateAsync(string inputText, string prompt, string targetLanguage)
    {
        var requestBody = new
        {
            model = _apiModel, 
            messages = new[]
            {
                new { role = "system", content = prompt },
                new { role = "user", content = inputText }
            }
        };

        var requestJson = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
        request.Headers.Add("Authorization", $"Bearer {_apiKey}");
        request.Content = requestJson;

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        var translatedText = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        return translatedText;
    }

    public string SetPrompt(string targetLanguage) {
        return
            $"You are a professional subtitle translator. " +
            $"Translate the following SRT subtitle file into **{targetLanguage}**. " +
            $"Follow these strict rules:\n\n" +

            $"1. **Preserve all numbering and timestamps exactly as they are**.\n" +
            $"2. **Only translate the spoken text lines** — never touch timestamps or line numbers.\n" +
            $"3. **The text under timestamps mostly consist of two lines. If the first line doesn't end with a dot (end of a sentence), " +
                $"then the sentence continues in the second line or next timestamp. " +
                $"Take that into consideration for the best translation quality. Especially when the text is in the upper caps. \n" +
            $"4. **Do not add any extra text, comments, introductions, or formatting**.\n" +
            $"5. **Keep all original line breaks and spacing** exactly as in the input.\n" +

            $"Before returning the result, double-check that:\n" +
            $"- All timestamps are unchanged.\n" +
            $"- All line numbers are present and in order.\n" +
            $"- No extra content was added.\n\n" +
            $"- Every word or sentence is translated to the {targetLanguage} language (if not, re-translate the whole text under that timestamp).\n\n" +

            $"Return only the translated subtitle file — no extra comments or explanations.";
    }
}
