using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "Gemini AI Gelişmiş Sentiment Analizi";
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("================================================");
        Console.WriteLine(" Gemini AI Gelişmiş Sentiment Analizi Uygulaması ");
        Console.WriteLine("================================================\n");
        Console.ResetColor();

        // API anahtarını appsettings.json'dan oku
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        var apiKey = config["GeminiApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("HATA: appsettings.json dosyasında 'GeminiApiKey' bulunamadı.");
            Console.WriteLine("Lütfen API anahtarınızı appsettings.json dosyasına ekleyin.");
            Console.ResetColor();
            return;
        }

        while (true)
        {
            // Kullanıcıdan metin al
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Lütfen gelişmiş duygu analizi yapılacak metni giriniz (çıkış için 'exit' yazın): ");
            Console.ResetColor();
            
            string input = Console.ReadLine()?.Trim() ?? "";

            // Çıkış kontrolü
            if (string.IsNullOrEmpty(input) || input.ToLower() == "exit")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nUygulama kapatılıyor. İyi günler!");
                Console.ResetColor();
                break;
            }

            // Gelişmiş duygu analizi işlemini başlat
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nGelişmiş duygu analizi yapılıyor...");
            Console.ResetColor();

            try
            {
                string analysis = await AdvancedSentimentalAnalysis(input, apiKey);
                
                // Sonucu göster
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nAnaliz Sonucu:");
                Console.ResetColor();
                Console.WriteLine(analysis);
                Console.WriteLine(new string('=', 60));
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                Console.ResetColor();
                Console.WriteLine(new string('=', 60));
                Console.WriteLine();
            }
        }
    }

    static async Task<string> AdvancedSentimentalAnalysis(string text, string apiKey)
    {
        using var httpClient = new HttpClient();
        
        // Gemini API endpoint
        var endpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";
        
        // API anahtarını header'a ekle
        httpClient.DefaultRequestHeaders.Add("X-goog-api-key", apiKey);

        // API isteği için request body hazırla - Gelişmiş duygu analizi prompt'u
        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { 
                            text = $@"Bu metnin gelişmiş duygu analizini yap ve aşağıdaki formatta JSON olarak döndür. Her duygu için 0-100 arası yüzdelik değer ver ve toplamları 100 olsun:

{{
  ""joy"": 0,
  ""sadness"": 0,
  ""anger"": 0,
  ""fear"": 0,
  ""surprise"": 0,
  ""neutral"": 0
}}

Analiz edilecek metin: ""{text}""

Sadece JSON formatında yanıt ver, başka açıklama yapma."
                        }
                    }
                }
            }
        };

        // JSON'a çevir
        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // API'ye istek gönder
        var response = await httpClient.PostAsync(endpoint, content);
        var responseString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            try
            {
                // JSON response'u parse et
                using var document = JsonDocument.Parse(responseString);
                var candidates = document.RootElement.GetProperty("candidates");
                if (candidates.GetArrayLength() > 0)
                {
                    var content_response = candidates[0].GetProperty("content");
                    var parts = content_response.GetProperty("parts");
                    if (parts.GetArrayLength() > 0)
                    {
                        var result = parts[0].GetProperty("text").GetString();
                        
                        return FormatAnalysisResult(result?.Trim() ?? "Analiz başarısız");
                    }
                }
                return "Analiz sonucu alınamadı";
            }
            catch (JsonException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("API yanıtı parse edilemedi.");
                Console.ResetColor();
                return "Parsing hatası";
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"API Hatası ({response.StatusCode}): {responseString}");
            Console.ResetColor();
            return "API Hatası";
        }
    }

    static string FormatAnalysisResult(string jsonResult)
    {
        try
        {
            // JSON içeriğini temizle (markdown kod bloklarını kaldır)
            var cleanJson = jsonResult.Replace("```json", "").Replace("```", "").Trim();
            
            // JSON'u parse et
            using var doc = JsonDocument.Parse(cleanJson);
            var root = doc.RootElement;

            var result = new StringBuilder();
            result.AppendLine("DETAYLI DUYGU ANALIZI:");
            result.AppendLine();
            
            // Ana duygular
            var emotions = new Dictionary<string, string>
            {
                ["joy"] = "Mutluluk",
                ["sadness"] = "Uzuntu",
                ["anger"] = "Ofke",
                ["fear"] = "Korku",
                ["surprise"] = "Sasirma",
                ["neutral"] = "Tarafsiz"
            };

            foreach (var emotion in emotions)
            {
                if (root.TryGetProperty(emotion.Key, out var emotionValue))
                {
                    var percentage = emotionValue.GetInt32();
                    var bar = GenerateProgressBar(percentage);
                    
                    Console.ForegroundColor = GetEmotionColor(emotion.Key, percentage);
                    result.AppendLine($"{emotion.Value,-10}: {percentage,3}% {bar}");
                    Console.ResetColor();
                }
            }

            return result.ToString();
        }
        catch (JsonException)
        {
            return $"Ham Analiz Sonucu:\n{jsonResult}";
        }
    }

    // Progress bar oluştur
    static string GenerateProgressBar(int percentage)
    {
        const int barLength = 20;
        var filledLength = (int)((double)percentage / 100 * barLength);
        var bar = new string('█', filledLength) + new string('░', barLength - filledLength);
        return $"[{bar}]";
    }

    static ConsoleColor GetEmotionColor(string emotion, int percentage)
    {
        if (percentage < 10) return ConsoleColor.DarkGray;
        
        return emotion switch
        {
            "joy" => ConsoleColor.Green,
            "sadness" => ConsoleColor.Blue,
            "anger" => ConsoleColor.Red,
            "fear" => ConsoleColor.DarkRed,
            "surprise" => ConsoleColor.Yellow,
            "neutral" => ConsoleColor.Gray,
            _ => ConsoleColor.White
        };
    }
}