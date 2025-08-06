using HtmlAgilityPack;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.Title = "Gemini AI Web Scraping Analizi";
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("===============================================");
        Console.WriteLine("  Gemini AI Destekli Web Scraping Uygulaması   ");
        Console.WriteLine("===============================================\n");
        Console.ResetColor();

        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // API anahtarını al ve kontrol et
        var apiKey = config["GeminiApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("HATA: appsettings.json içinde 'GeminiApiKey' bulunamadı.");
            Console.WriteLine("Lütfen API anahtarınızı appsettings.json dosyasına ekleyin.");
            Console.ResetColor();
            Console.WriteLine("\nÇıkmak için bir tuşa basın...");
            Console.ReadKey();
            return;
        }

        while (true)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Lütfen analiz yapmak istediğiniz web sayfa URL'ini giriniz (çıkış için 'exit'): ");
                Console.ResetColor();

                string? inputUrl = Console.ReadLine()?.Trim();

                // Çıkış kontrolü
                if (string.IsNullOrEmpty(inputUrl) || inputUrl.ToLower() == "exit")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nUygulama kapatılıyor. İyi günler!");
                    Console.ResetColor();
                    break;
                }

                // URL formatını kontrol et
                if (!IsValidUrl(inputUrl))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Geçerli bir URL giriniz! (örn: https://www.example.com)");
                    Console.ResetColor();
                    Console.WriteLine();
                    continue;
                }

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\nWeb sayfası içeriği çekiliyor...");
                Console.ResetColor();

                string webContent = ExtractTextFromWeb(inputUrl);

                if (webContent == "Sayfa içeriği okunamadı.")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Web sayfası içeriği okunamadı. URL'yi kontrol edin.");
                    Console.ResetColor();
                    Console.WriteLine();
                    continue;
                }

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("AI analizi yapılıyor...");
                Console.ResetColor();

                await AnalyzeWithGeminiAI(webContent, "Web Sayfası İçeriği", apiKey);

                Console.WriteLine(new string('=', 70));
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                Console.ResetColor();
                Console.WriteLine();
            }
        }
    }

    static string ExtractTextFromWeb(string url)
    {
        try
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var bodyText = doc.DocumentNode.SelectSingleNode("//body")?.InnerText;

            // Metni temizle (gereksiz boşlukları kaldır)
            if (!string.IsNullOrEmpty(bodyText))
            {
                bodyText = System.Text.RegularExpressions.Regex.Replace(bodyText, @"\s+", " ").Trim();

                // Çok uzun metinleri kısalt (Gemini API sınırları için)
                if (bodyText.Length > 8000)
                {
                    bodyText = bodyText.Substring(0, 8000) + "...";
                }
            }

            return bodyText ?? "Sayfa içeriği okunamadı.";
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Web scraping hatası: {ex.Message}");
            Console.ResetColor();
            return "Sayfa içeriği okunamadı.";
        }
    }

    static async Task AnalyzeWithGeminiAI(string text, string sourceType, string apiKey)
    {
        using var httpClient = new HttpClient();

        // Gemini API endpoint
        var endpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";

        // API anahtarını header'a ekle
        httpClient.DefaultRequestHeaders.Add("X-goog-api-key", apiKey);

        // API'ye gönderilecek istek gövdesini hazırla
        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new {
                            text = $"Lütfen aşağıdaki {sourceType} metnini analiz ederek özetleyin. Özeti Türkçe olarak yazın ve şunları dahil edin:\n" +
                                   "1. Ana konular ve önemli noktalar\n" +
                                   "2. Metnin genel amacı\n" +
                                   "3. Öne çıkan bilgiler\n" +
                                   "4. Varsa sayısal veriler\n\n" +
                                   $"Analiz edilecek metin:\n{text}"
                        }
                    }
                }
            }
        };

        try
        {
            // İstek gövdesini JSON formatına çevir
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // API'ye POST isteği gönder
            var response = await httpClient.PostAsync(endpoint, content);
            var responseString = await response.Content.ReadAsStringAsync();

            // İstek başarılı mı kontrol et
            if (response.IsSuccessStatusCode)
            {
                // JSON yanıtını parse et
                var responseJson = JsonDocument.Parse(responseString);
                var candidates = responseJson.RootElement.GetProperty("candidates");

                // Yanıtta sonuç var mı kontrol et
                if (candidates.GetArrayLength() > 0)
                {
                    var firstCandidate = candidates[0];
                    var contentProp = firstCandidate.GetProperty("content");
                    var parts = contentProp.GetProperty("parts");

                    // Analiz sonucunu al ve göster
                    if (parts.GetArrayLength() > 0)
                    {
                        var analysis = parts[0].GetProperty("text").GetString();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\nGemini AI Analizi ({sourceType}):");
                        Console.ResetColor();
                        Console.WriteLine(analysis?.Trim() ?? "Analiz sonucu alınamadı.");
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"API Hatası: {response.StatusCode}");
                Console.WriteLine($"Hata detayı: {responseString}");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"AI analiz hatası: {ex.Message}");
            Console.ResetColor();
        }
    }

    static bool IsValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri? result)
               && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }
}
