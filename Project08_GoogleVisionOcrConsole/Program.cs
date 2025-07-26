using Google.Cloud.Vision.V1;
using System.Text;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "Google Vision OCR - Metin Tanıma";
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== Google Vision OCR Metin Tanıma Uygulaması ===\n");
        Console.ResetColor();

        try
        {
            // Google Cloud kimlik bilgilerini (credentials) ayarlıyoruz
            // Bu adım Google API'sine bağlanmak için zorunlu
            if (!SetupGoogleCredentials())
            {
                Console.WriteLine("Uygulama kapatılıyor...");
                Console.ReadKey();
                return;
            }

            // Ana uygulama döngüsü - kullanıcı çıkana kadar çalışır
            bool continueApp = true;
            while (continueApp)
            {
                // Menüyü göster ve kullanıcı seçimini al
                ShowMainMenu();
                string choice = Console.ReadLine()?.Trim();

                // Kullanıcının seçimine göre işlem yap
                switch (choice)
                {
                    case "1":
                        // Images klasöründeki resimlerden birini seç ve işle
                        await ProcessImageFromGallery();
                        break;
                    case "2":
                        // Kullanıcının verdiği dosya yolundaki resmi işle
                        await ProcessImageFromCustomPath();
                        break;
                    case "3":
                    case "q":
                    case "quit":
                    case "çık":
                        // Uygulamadan çık
                        continueApp = false;
                        break;
                    default:
                        // Geçersiz seçim durumunda hata mesajı göster
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Geçersiz seçim! Lütfen 1-3 arasında bir seçim yapın.\n");
                        Console.ResetColor();
                        break;
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Uygulama kapatılıyor. Teşekkürler!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            // Beklenmeyen hataları yakala ve kullanıcıya göster
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Beklenmeyen bir hata oluştu: {ex.Message}");
            Console.ResetColor();
            Console.ReadKey();
        }
    }

    // Google Cloud kimlik bilgilerini (credentials) ayarlayan fonksiyon
    private static bool SetupGoogleCredentials()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Google Cloud credentials dosyası ayarlanıyor...");
        Console.ResetColor();

        // Google Cloud'dan indirilen JSON dosyasının adı
        // Bu dosyayı proje klasörüne koymanız gerekiyor
        string credentialFileName = "famous-modem-465613-k1-bf89e7e629b6.json";
        string credentialPath = Path.Combine(Directory.GetCurrentDirectory(), credentialFileName);

        // JSON dosyasının var olup olmadığını kontrol et
        if (!File.Exists(credentialPath))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"HATA: Google Cloud credentials dosyası bulunamadı!");
            Console.WriteLine($"Aranan dosya: {credentialFileName}");
            Console.WriteLine("Lütfen bu dosyayı proje klasörüne kopyalayın.");
            Console.ResetColor();
            return false; // Dosya yoksa işlem başarısız
        }

        try
        {
            // Google Cloud API'si için environment variable (ortam değişkeni) ayarla
            // Bu sayede Google API'si kimlik doğrulaması yapabilir
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Credentials dosyası ayarlandı: {credentialFileName}");
            Console.ResetColor();
            return true;
        }
        catch (Exception ex)
        {
            // Kimlik bilgileri ayarlanırken hata oluştuysa
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Credentials dosyası ayarlanırken hata oluştu: {ex.Message}");
            Console.ResetColor();
            return false;
        }
    }

    // Ana menüyü ekranda gösteren fonksiyon
    private static void ShowMainMenu()
    {
        Console.WriteLine("\n" + new string('=', 50));
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Ana Menü:");
        Console.WriteLine("1. Images klasöründen resim seç");
        Console.WriteLine("2. Özel dosya yolu gir");
        Console.WriteLine("3. Çıkış");
        Console.ResetColor();
        Console.WriteLine(new string('=', 50));
        Console.Write("Seçiminizi yapın: ");
    }

    // Images klasöründeki resimleri listeleyen ve seçim yaptıran fonksiyon
    private static async Task ProcessImageFromGallery()
    {
        // Proje klasörü ve Images alt klasörünün yollarını belirle
        string projectDirectory = Directory.GetCurrentDirectory();
        string imagesFolder = Path.Combine(projectDirectory, "Images");

        // Images klasörünün var olup olmadığını kontrol et
        if (!Directory.Exists(imagesFolder))
        {
            // Yoksa oluştur ve kullanıcıyı bilgilendir
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Images klasörü bulunamadı. Klasör oluşturuluyor...");
            Directory.CreateDirectory(imagesFolder);
            Console.WriteLine("Images klasörü oluşturuldu. Lütfen test resimlerinizi bu klasöre koyun.");
            Console.ResetColor();
            return;
        }

        // Desteklenen resim formatlarının listesi
        string[] imageExtensions = { "*.jpg", "*.jpeg", "*.png", "*.bmp", "*.gif", "*.tiff", "*.webp" };
        var imageFiles = new List<string>(); // Bulunan resim dosyalarını saklayacak liste

        // Her resim formatı için Images klasöründe arama yap
        foreach (string extension in imageExtensions)
        {
            // Alt klasörlerde de arama yap (SearchOption.AllDirectories)
            imageFiles.AddRange(Directory.GetFiles(imagesFolder, extension, SearchOption.AllDirectories));
        }

        // Hiç resim bulunamadıysa kullanıcıyı bilgilendir
        if (imageFiles.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Images klasöründe desteklenen resim dosyası bulunamadı.");
            Console.WriteLine("Desteklenen formatlar: JPG, JPEG, PNG, BMP, GIF, TIFF, WEBP");
            Console.ResetColor();
            return;
        }

        // Bulunan resimleri numaralı liste halinde göster
        Console.WriteLine($"\nImages klasöründe {imageFiles.Count} resim bulundu:");
        Console.WriteLine(new string('-', 40)); // Görsel ayırıcı

        // Her resim için numara ve dosya adını yazdır
        for (int i = 0; i < imageFiles.Count; i++)
        {
            string fileName = Path.GetFileName(imageFiles[i]);
            Console.WriteLine($"{i + 1}. {fileName}");
        }

        Console.WriteLine(new string('-', 40));
        Console.Write($"Resim seçin (1-{imageFiles.Count}): ");

        // Kullanıcının girdiği numarayı kontrol et ve geçerliyse resmi işle
        if (int.TryParse(Console.ReadLine(), out int selection) &&
            selection >= 1 && selection <= imageFiles.Count)
        {
            string selectedImage = imageFiles[selection - 1]; // Array 0'dan başladığı için -1
            await ProcessImage(selectedImage); // Seçilen resmi OCR işlemine gönder
        }
        else
        {
            // Geçersiz numara girildiyse hata mesajı
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Geçersiz seçim!");
            Console.ResetColor();
        }
    }

    // Kullanıcının girdiği dosya yolundaki resmi işleyen fonksiyon
    private static async Task ProcessImageFromCustomPath()
    {
        Console.Write("\nResim dosyasının tam yolunu girin: ");
        string imagePath = Console.ReadLine()?.Trim(); // Başındaki ve sonundaki boşlukları temizle

        // Boş girdi kontrolü
        if (string.IsNullOrEmpty(imagePath))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Dosya yolu boş olamaz!");
            Console.ResetColor();
            return;
        }

        // Dosyanın gerçekten var olup olmadığını kontrol et
        if (!File.Exists(imagePath))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Dosya bulunamadı!");
            Console.ResetColor();
            return;
        }

        // Dosya varsa OCR işlemine gönder
        await ProcessImage(imagePath);
    }

    // Seçilen resim üzerinde OCR (metin tanıma) işlemi yapan ana fonksiyon
    private static async Task ProcessImage(string imagePath)
    {
        try
        {
            // İşlem başladığını kullanıcıya bildir
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nİşlem başlatılıyor...");
            Console.WriteLine($"Dosya: {Path.GetFileName(imagePath)}");
            Console.ResetColor();

            // Google Vision API client'ını oluştur
            var client = await ImageAnnotatorClient.CreateAsync();
            
            // Resmi Google Vision API'sine gönderebilmek için Image nesnesine çevir
            var image = Image.FromFile(imagePath);
            
            // Google Vision API'sinden metin tespit etmesini iste
            var response = await client.DetectTextAsync(image);

            Console.WriteLine("\n" + new string('=', 60));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("SONUÇ:");
            Console.ResetColor();
            Console.WriteLine(new string('-', 60));

            // API'den gelen sonuçları kontrol et
            if (response.Count > 0)
            {
                // Her tespit edilen metin parçası için
                foreach (var annotation in response)
                {
                    // Boş olmayan metinleri ekrana yazdır
                    if (!string.IsNullOrEmpty(annotation.Description))
                    {
                        Console.WriteLine(annotation.Description);
                        Console.WriteLine();
                    }
                }
            }
            else
            {
                // Hiç metin tespit edilemediğinde
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Bu resimde metin tespit edilemedi.");
                Console.ResetColor();
            }

            Console.WriteLine(new string('=', 60));

            // Kullanıcıya sonuçları dosyaya kaydetme seçeneği sun
            OfferToSaveResults(response, imagePath);
        }
        catch (Exception ex)
        {
            // OCR işlemi sırasında hata oluştuysa
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"İşlem sırasında hata oluştu: {ex.Message}");
            Console.ResetColor();
        }
    }

    // OCR sonuçlarını dosyaya kaydetme seçeneği sunan fonksiyon
    private static void OfferToSaveResults(IReadOnlyList<EntityAnnotation> response, string imagePath)
    {
        // Hiç sonuç yoksa kaydetmeye gerek yok
        if (response.Count == 0) return;

        // Kullanıcıya kaydetmek isteyip istemediğini sor
        Console.Write("Sonuçları dosyaya kaydet? (e/h): ");
        string answer = Console.ReadLine()?.ToLower().Trim();

        if (answer == "e" || answer == "evet" || answer == "y" || answer == "yes")
        {
            try
            {
                // Sonuçları kaydedeceğimiz klasörü hazırla
                string projectDirectory = Directory.GetCurrentDirectory();
                string resultsFolder = Path.Combine(projectDirectory, "Results");

                // Results klasörü yoksa oluştur
                if (!Directory.Exists(resultsFolder))
                {
                    Directory.CreateDirectory(resultsFolder);
                }

                // Dosya adını tarih-saat ile benzersiz yap
                string fileName = $"OCR_Result_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                string resultPath = Path.Combine(resultsFolder, fileName);

                // Dosya içeriğini hazırla
                var content = new StringBuilder();
                content.AppendLine("=== GOOGLE VISION OCR SONUCU ===");
                content.AppendLine($"Tarih: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                content.AppendLine($"Kaynak: {Path.GetFileName(imagePath)}");
                content.AppendLine($"Tam Yol: {imagePath}");
                content.AppendLine();
                content.AppendLine("=== TESPİT EDİLEN METİN ===");
                content.AppendLine(new string('-', 40));

                // Her tespit edilen metin parçasını dosyaya ekle
                foreach (var annotation in response)
                {
                    if (!string.IsNullOrEmpty(annotation.Description))
                    {
                        content.AppendLine(annotation.Description);
                        content.AppendLine();
                    }
                }

                File.WriteAllText(resultPath, content.ToString(), System.Text.Encoding.UTF8);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Sonuç kaydedildi: {resultPath}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Kaydetme hatası: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}