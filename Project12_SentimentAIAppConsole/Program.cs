using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "Gemini AI Sentiment Analizi";
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("========================================");
        Console.WriteLine(" Gemini AI Sentiment Analizi Uygulaması ");
        Console.WriteLine("========================================\n");
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
            // Kullanıcıdan metni al
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Lütfen duygu analizi yapılacak metni giriniz (çıkış yapmak için 'exit' yazın): ");
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

            // Duygu analizi işlemini başlat
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nDuygu analizi yapılıyor...");
            Console.ResetColor();

            try
            {
                string sentiment = await AnalyzeSentimentAsync(input, apiKey);

                Console.ForegroundColor = GetSentimentColor(sentiment);
                Console.WriteLine($"Sonuç: {sentiment}");
                Console.ResetColor();
                Console.WriteLine(new string('-', 50));
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                Console.ResetColor();
                Console.WriteLine(new string('-', 50));
                Console.WriteLine();
            }
        }
    }

    static async Task<string> AnalyzeSentimentAsync(string text, string apiKey)
    {
        using var httpClient = new HttpClient();

        // Gemini API endpoint
        var endpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";

        // API anahtarını header'a ekle
        httpClient.DefaultRequestHeaders.Add("X-goog-api-key", apiKey);

        // API isteği için request body hazırla
        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new {
                            text = $"Bu metnin duygusal tonunu analiz et ve sadece 'Pozitif', 'Negatif' veya 'Nötr' kelimelerinden birini döndür. Başka bir açıklama yapma, sadece duygu kategorisini belirt: \"{text}\""
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
                        return NormalizeSentiment(result?.Trim() ?? "Belirsiz");
                    }
                }
                return "Belirsiz";
            }
            catch (JsonException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("API yanıtı parse edilemedi.");
                Console.ResetColor();
                return "Hata";
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

    // Sentiment sonucunu normalize et
    static string NormalizeSentiment(string sentiment)
    {
        var normalized = sentiment.ToLower().Trim();

        if (normalized.Contains("pozitif") || normalized.Contains("positive") || normalized.Contains("olumlu"))
            return "Pozitif";
        else if (normalized.Contains("negatif") || normalized.Contains("negative") || normalized.Contains("olumsuz"))
            return "Negatif";
        else if (normalized.Contains("nötr") || normalized.Contains("neutral") || normalized.Contains("tarafsız"))
            return "Nötr";
        else
            return sentiment;
    }

    // Sentiment sonucuna göre renk belirle
    static ConsoleColor GetSentimentColor(string sentiment)
    {
        return sentiment.ToLower() switch
        {
            var s when s.Contains("pozitif") => ConsoleColor.Green,
            var s when s.Contains("negatif") => ConsoleColor.Red,
            var s when s.Contains("nötr") => ConsoleColor.Yellow,
            _ => ConsoleColor.White
        };
    }
}