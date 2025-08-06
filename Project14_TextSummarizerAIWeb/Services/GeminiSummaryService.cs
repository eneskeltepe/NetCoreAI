using System.Text;
using System.Text.Json;
using Project14_TextSummarizerAIWeb.Models;

namespace Project14_TextSummarizerAIWeb.Services;

public class GeminiSummaryService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _endpoint;
    private readonly ILogger<GeminiSummaryService> _logger;

    public GeminiSummaryService(HttpClient httpClient, IConfiguration configuration, ILogger<GeminiSummaryService> logger)
    {
        _httpClient = httpClient;
        _apiKey = configuration["GeminiApiKey"] ?? throw new ArgumentNullException("GeminiApiKey not found");
        _endpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";
        _logger = logger;
        
        _httpClient.DefaultRequestHeaders.Add("X-goog-api-key", _apiKey);
    }

    public async Task<SummaryResult> SummarizeTextAsync(SummaryRequest request)
    {
        try
        {
            var result = new SummaryResult
            {
                Language = request.Language,
                OriginalCharCount = request.Text.Length,
                OriginalWordCount = CountWords(request.Text),
                OriginalTextPreview = request.Text.Length > 200 
                    ? request.Text.Substring(0, 200) + "..." 
                    : request.Text
            };

            // 1. Generate Title First
            result.Title = await GenerateTitleAsync(request.Text, request.Language);

            // 2. Generate Summaries
            var summaryTasks = new List<Task<(string type, string content)>>();

            if (request.EnableShort)
            {
                summaryTasks.Add(GenerateSummaryAsync(request.Text, "short", request.Language));
            }

            if (request.EnableMedium)
            {
                summaryTasks.Add(GenerateSummaryAsync(request.Text, "medium", request.Language));
            }

            if (request.EnableDetailed)
            {
                summaryTasks.Add(GenerateSummaryAsync(request.Text, "detailed", request.Language));
            }

            var summaryResults = await Task.WhenAll(summaryTasks);

            foreach (var (type, content) in summaryResults)
            {
                switch (type)
                {
                    case "short":
                        result.ShortSummary = content;
                        break;
                    case "medium":
                        result.MediumSummary = content;
                        break;
                    case "detailed":
                        result.DetailedSummary = content;
                        break;
                }
            }

            result.IsSuccess = true;
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in SummarizeTextAsync");
            return new SummaryResult
            {
                IsSuccess = false,
                ErrorMessage = "Özet oluşturulurken bir hata oluştu. Lütfen tekrar deneyin."
            };
        }
    }

    private async Task<string> GenerateTitleAsync(string text, string language)
    {
        try
        {
            var instruction = language == "tr" 
                ? "Bu metni özetleyen kısa, çarpıcı ve açıklayıcı bir başlık oluştur. Maksimum 60 karakter. Sadece başlığı döndür, başka bir şey yazma."
                : "Create a short, compelling and descriptive title that summarizes this text. Maximum 60 characters. Return only the title, nothing else.";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = $"{instruction}\n\n{text}" }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_endpoint, content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                using var document = JsonDocument.Parse(responseString);
                var candidates = document.RootElement.GetProperty("candidates");
                if (candidates.GetArrayLength() > 0)
                {
                    var contentResponse = candidates[0].GetProperty("content");
                    var parts = contentResponse.GetProperty("parts");
                    if (parts.GetArrayLength() > 0)
                    {
                        var title = parts[0].GetProperty("text").GetString() ?? "Başlıksız Özet";
                        // Clean and limit title
                        title = title.Trim().Replace("\"", "").Replace("'", "");
                        return title.Length > 60 ? title.Substring(0, 60) + "..." : title;
                    }
                }
            }

            return language == "tr" ? "Başlıksız Özet" : "Untitled Summary";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating title");
            return language == "tr" ? "Başlıksız Özet" : "Untitled Summary";
        }
    }

    private async Task<(string type, string content)> GenerateSummaryAsync(string text, string type, string language)
    {
        var instruction = type switch
        {
            "short" => language == "tr" ? "Bu metni 1-2 cümle ile özetle." : "Summarize this text in 1-2 sentences.",
            "medium" => language == "tr" ? "Bu metni 3-5 cümle ile özetle." : "Summarize this text in 3-5 sentences.",
            "detailed" => language == "tr" ? "Bu metni detaylı ama özlü bir şekilde özetle." : "Summarize this text in a detailed but concise manner.",
            _ => "Summarize this text."
        };

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = $"{instruction}\n\n{text}" }
                    }
                }
            }
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(_endpoint, content);
        var responseString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            using var document = JsonDocument.Parse(responseString);
            var candidates = document.RootElement.GetProperty("candidates");
            if (candidates.GetArrayLength() > 0)
            {
                var contentResponse = candidates[0].GetProperty("content");
                var parts = contentResponse.GetProperty("parts");
                if (parts.GetArrayLength() > 0)
                {
                    return (type, parts[0].GetProperty("text").GetString() ?? "Özet oluşturulamadı");
                }
            }
        }

        throw new Exception($"API Error: {response.StatusCode}");
    }

    private static int CountWords(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return 0;

        return text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }
}