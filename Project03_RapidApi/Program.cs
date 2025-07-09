using Newtonsoft.Json;
using Project03_RapidApi.ViewModels;
using System.Net.Http;

// API'den dizi listesini çeken ve ekrana yazdıran fonksiyon
async Task GetAndPrintSeriesAsync()
{
    // HttpClient nesnesi oluşturuluyor
    using var client = new HttpClient();

    // API isteği için gerekli bilgiler hazırlanıyor
    var request = new HttpRequestMessage
    {
        Method = HttpMethod.Get,
        RequestUri = new Uri("https://imdb-top-100-movies.p.rapidapi.com/series/"),
        Headers =
        {
            { "x-rapidapi-key", "4836cca031msh78cda1210a105d2p1d25acjsn645522531b4d" },
            { "x-rapidapi-host", "imdb-top-100-movies.p.rapidapi.com" },
        },
    };

    try
    {
        // API'ye istek gönderiliyor
        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        // Gelen yanıt okunuyor
        var body = await response.Content.ReadAsStringAsync();

        // JSON verisi modele dönüştürülüyor
        var apiSeriesViewModels = JsonConvert.DeserializeObject<List<ApiSeriesViewModel>>(body);

        // Sonuçlar ekrana yazdırılıyor
        foreach (var series in apiSeriesViewModels)
        {
            Console.WriteLine($"Rank: {series.rank}");
            Console.WriteLine($"Title: {series.title}");
            Console.WriteLine($"Description: {series.description}");
            Console.WriteLine($"Rating: {series.rating}");
            Console.WriteLine($"Year: {series.year}");
            Console.WriteLine(new string('-', 50));
        }
    }
    catch (Exception ex)
    {
        // Hata durumunda kullanıcı bilgilendiriliyor
        Console.WriteLine("Bir hata oluştu: " + ex.Message);
    }
}

await GetAndPrintSeriesAsync();

Console.WriteLine("Çıkmak için bir tuşa basınız...");
Console.ReadLine();