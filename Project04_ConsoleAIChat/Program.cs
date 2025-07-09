using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

class Program
{
    static async Task Main(string[] args)
    {
        // 1. appsettings.json'dan API anahtarını okuyoruz
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        var apiKey = config["GeminiApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("HATA: appsettings.json içinde 'GeminiApiKey' bulunamadı.");
            Console.WriteLine("Lütfen API anahtarınızı appsettings.json dosyasına ekleyin.");
            Console.ResetColor();
            return;
        }

        // 2. HttpClient ve endpoint ayarları
        using var httpClient = new HttpClient();
        var endpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";
        httpClient.DefaultRequestHeaders.Add("X-goog-api-key", apiKey);

        // 3. Sohbet geçmişini tutmak için bir liste oluşturuyoruz
        var chatHistory = new List<(string Sender, string Message)>();

        // 4. Kullanıcıya hoş geldin mesajı
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Gemini Chatbot'a hoş geldiniz!");
        Console.WriteLine("Çıkmak için 'çık' veya 'exit' yazabilirsiniz.\n");
        Console.ResetColor();

        // 5. Sonsuz döngü ile sohbeti başlatıyoruz
        while (true)
        {
            // Kullanıcıdan mesaj al
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Siz: ");
            Console.ResetColor();
            var prompt = Console.ReadLine();

            // Çıkış kontrolü
            if (string.IsNullOrWhiteSpace(prompt) || prompt.Trim().ToLower() == "çık" || prompt.Trim().ToLower() == "exit")
                break;

            // Kullanıcı mesajını geçmişe ekle
            chatHistory.Add(("Siz", prompt));

            // API'ye gönderilecek istek gövdesi
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
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                // API'ye isteği gönder
                var response = await httpClient.PostAsync(endpoint, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // API'dan gelen cevabı ayrıştır
                    var result = JsonSerializer.Deserialize<JsonElement>(responseString);
                    var answer = result
                        .GetProperty("candidates")[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString();

                    // Gemini cevabını geçmişe ekle
                    chatHistory.Add(("Gemini", answer));

                    // Sohbet geçmişini ekrana temiz ve renkli şekilde yazdır
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Gemini Chatbot'a hoş geldiniz!");
                    Console.WriteLine("Çıkmak için 'çık' veya 'exit' yazabilirsiniz.\n");
                    Console.ResetColor();

                    foreach (var (Sender, Message) in chatHistory)
                    {
                        if (Sender == "Siz")
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("Siz: ");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("Gemini: ");
                        }
                        Console.ResetColor();
                        Console.WriteLine(Message + "\n");
                    }
                }
                else
                {
                    // API'dan hata dönerse kullanıcıya bildir
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Hata: {response.StatusCode}");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                // Diğer hataları kullanıcıya bildir
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Hata: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}