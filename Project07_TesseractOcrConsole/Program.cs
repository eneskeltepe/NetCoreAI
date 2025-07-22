using System;
using System.IO;
using Tesseract;

class Program
{
    // Tesseract veri dosyalarının bulunduğu klasör yolu
    private static readonly string tessDataPath = @"C:\tessdata";

    static void Main(string[] args)
    {
        // Konsol penceresi başlığını ayarla
        Console.Title = "Tesseract OCR Uygulaması";
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== Tesseract OCR Uygulaması ===\n");
        Console.ResetColor();

        // Ana menü döngüsü - kullanıcı çıkana kadar devam eder
        bool continueApp = true;
        while (continueApp)
        {
            ShowMainMenu();
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    StartOcrProcess(); // OCR işlemini başlat
                    break;
                case "2":
                    ShowHelpMenu(); // Yardım menüsünü göster
                    break;
                case "3":
                case "q":
                case "quit":
                case "çık":
                    continueApp = false; // Uygulamadan çık
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Geçersiz seçim! Lütfen 1, 2 veya 3 girin.\n");
                    Console.ResetColor();
                    break;
            }
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Uygulama kapatılıyor. Teşekkürler!");
        Console.ResetColor();
    }

    // Ana menüyü ekranda gösterir
    private static void ShowMainMenu()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Ana Menü:");
        Console.WriteLine("1. OCR İşlemi Başlat");
        Console.WriteLine("2. Yardım ve Kurulum");
        Console.WriteLine("3. Çıkış");
        Console.ResetColor();
        Console.Write("\nSeçiminizi yapın: ");
    }

    // Yardım ve kurulum rehberini ekranda gösterir
    private static void ShowHelpMenu()
    {
        Console.Clear();
        var helpText = @"
=== KURULUM REHBERİ ===

KURULUM ADIMLARI:
1. C:\ sürücüsünde 'tessdata' klasörü oluşturun
2. Dil dosyalarını indirin ve C:\tessdata\ klasörüne kopyalayın

DİL DOSYALARI:
İngilizce (zorunlu): https://github.com/tesseract-ocr/tessdata/raw/main/eng.traineddata
Türkçe (önerilen): https://github.com/tesseract-ocr/tessdata/raw/main/tur.traineddata
Diğer diller: https://github.com/tesseract-ocr/tessdata

GÖRÜNTÜ DOSYALARI:
• Test resimlerini proje klasörüne veya alt klasörlere koyun. (JPG, PNG, BMP, TIFF, GIF gibi formatlar desteklenir)

GÜVENİLİRLİK SKORLARI:
90-100%: Mükemmel  |  80-89%: İyi  |  60-79%: Orta  |  40-59%: Zayıf  |  0-39%: Çok zayıf";

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(helpText);
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Ana menüye dönmek için herhangi bir tuşa basın...");
        Console.ResetColor();
        Console.ReadKey();
        Console.Clear();
    }

    // OCR işlemini başlatır ve kullanıcıyı adım adım yönlendirir
    private static void StartOcrProcess()
    {
        // Önce sistem dosyalarını kontrol et
        if (!CheckTesseractData())
        {
            Console.WriteLine("\nAna menüye dönmek için herhangi bir tuşa basın...");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        // OCR işlemi döngüsü - kullanıcı istediği kadar dosya işleyebilir
        bool continueProcessing = true;
        while (continueProcessing)
        {
            try
            {
                // 1. Dil seçimi
                string selectedLanguage = SelectLanguage();

                // 2. Dosya yolu alma
                string imagePath = GetImagePath();
                if (string.IsNullOrEmpty(imagePath))
                {
                    continue; // Dosya seçilmediyse tekrar dene
                }

                // 3. OCR işlemini gerçekleştir
                PerformOCR(imagePath, selectedLanguage);

                // 4. Devam etmek isteyip istemediğini sor
                continueProcessing = AskToContinue();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nBir hata oluştu: {ex.Message}");
                Console.ResetColor();

                continueProcessing = AskToContinue();
            }
        }
        Console.Clear(); // Ana menüye dönmeden önce ekranı temizle
    }

    // Tesseract veri dosyalarının varlığını kontrol eder
    private static bool CheckTesseractData()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Sistem dosyaları kontrol ediliyor...");
        Console.ResetColor();

        // tessdata klasörünün varlığını kontrol et
        if (!Directory.Exists(tessDataPath))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"HATA: Gerekli veri klasörü bulunamadı: {tessDataPath}");
            Console.WriteLine("Kurulum için '2 - Yardım ve Kurulum' menüsünü kontrol edin.");
            Console.ResetColor();
            return false;
        }

        // İngilizce dil dosyasını kontrol et (zorunlu)
        string engFile = Path.Combine(tessDataPath, "eng.traineddata");
        if (!File.Exists(engFile))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"HATA: İngilizce dil dosyası bulunamadı: {engFile}");
            Console.WriteLine("Kurulum için '2 - Yardım ve Kurulum' menüsünü kontrol edin.");
            Console.ResetColor();
            return false;
        }

        // Türkçe dil dosyasını kontrol et (isteğe bağlı)
        string turFile = Path.Combine(tessDataPath, "tur.traineddata");
        if (!File.Exists(turFile))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"UYARI: Türkçe dil dosyası bulunamadı: {turFile}");
            Console.WriteLine("Türkçe desteği için kurulum menüsünü kontrol edin.");
            Console.ResetColor();
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Sistem hazır!\n");
        Console.ResetColor();
        return true;
    }

    // Kullanıcıdan OCR işlemi için dil seçimini alır
    private static string SelectLanguage()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Dil Seçenekleri:");
        Console.WriteLine("1. İngilizce");
        Console.WriteLine("2. Türkçe");
        Console.WriteLine("3. İngilizce + Türkçe");
        Console.WriteLine("4. Otomatik");
        Console.ResetColor();

        Console.Write("Seçiminizi yapın (1-4): ");
        string choice = Console.ReadLine();

        return choice switch
        {
            "1" => "eng",
            "2" => "tur",
            "3" => "eng+tur",
            "4" => "eng+tur", // Otomatik tespit için çoklu dil kullan
            _ => "eng+tur" // Varsayılan seçenek
        };
    }

    // Kullanıcıdan işlenecek görüntü dosyasının yolunu alır
    private static string GetImagePath()
    {
        Console.WriteLine("\nDosya Seçimi:");
        Console.WriteLine("1. Dosya yolunu gir");
        Console.WriteLine("2. Klasörden seç");

        Console.Write("Seçiminizi yapın (1-2): ");
        string choice = Console.ReadLine();

        if (choice == "2")
        {
            return SelectFromCurrentDirectory(); // Klasörden dosya seç
        }
        else
        {
            // Manuel yol girme
            Console.Write("\nGörüntü dosyasının yolunu girin: ");
            string path = Console.ReadLine();

            if (!File.Exists(path))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Dosya bulunamadı!");
                Console.ResetColor();
                return null;
            }

            return path;
        }
    }

    // Proje klasöründeki tüm görüntü dosyalarını arar ve kullanıcıya listeler
    private static string SelectFromCurrentDirectory()
    {
        // Desteklenen görüntü formatları
        string[] imageExtensions = { "*.jpg", "*.jpeg", "*.png", "*.bmp", "*.tiff", "*.gif" };
        var imageFiles = new List<string>();

        // Proje dizinini al
        string projectDirectory = Directory.GetCurrentDirectory();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Görüntü dosyaları aranıyor...");
        Console.ResetColor();

        // Tüm alt klasörlerde görüntü dosyalarını ara
        foreach (string extension in imageExtensions)
        {
            try
            {
                imageFiles.AddRange(Directory.GetFiles(projectDirectory, extension, SearchOption.AllDirectories));
            }
            catch (Exception ex)
            {
                // Erişim hatalarını görmezden gel, devam et
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Arama hatası ({extension}): {ex.Message}");
                Console.ResetColor();
            }
        }

        // Dosya bulunamadıysa uyar
        if (imageFiles.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Proje klasöründe görüntü dosyası bulunamadı.");
            Console.WriteLine($"Aranan konum: {projectDirectory}");
            Console.ResetColor();
            return null;
        }

        // Dosyaları klasör gruplarına ayır
        var groupedFiles = imageFiles
            .GroupBy(file => Path.GetDirectoryName(file))
            .OrderBy(group => group.Key)
            .ToList();

        Console.WriteLine($"\nBulunan {imageFiles.Count} görüntü dosyası:");
        Console.WriteLine(new string('=', 60));

        int fileIndex = 1;
        var indexToFileMap = new Dictionary<int, string>();

        // Dosyaları klasör bazında listele
        foreach (var group in groupedFiles)
        {
            string relativePath = Path.GetRelativePath(projectDirectory, group.Key);
            if (string.IsNullOrEmpty(relativePath) || relativePath == ".")
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("📁 Proje Ana Klasörü:");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"📁 {relativePath}\\:");
                Console.ResetColor();
            }

            // Klasördeki dosyaları listele
            foreach (var file in group.OrderBy(f => Path.GetFileName(f)))
            {
                indexToFileMap[fileIndex] = file;
                Console.WriteLine($"   {fileIndex}. {Path.GetFileName(file)}");
                fileIndex++;
            }
            Console.WriteLine();
        }

        Console.WriteLine(new string('=', 60));
        Console.Write($"Dosya seçin (1-{imageFiles.Count}): ");

        // Kullanıcının seçimini işle
        if (int.TryParse(Console.ReadLine(), out int selection) &&
            selection >= 1 && selection <= imageFiles.Count &&
            indexToFileMap.ContainsKey(selection))
        {
            string selectedFile = indexToFileMap[selection];
            string relativeSelectedPath = Path.GetRelativePath(projectDirectory, selectedFile);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Seçilen: {relativeSelectedPath}");
            Console.ResetColor();

            return selectedFile;
        }

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Geçersiz seçim!");
        Console.ResetColor();
        return null;
    }

    // Asıl OCR işlemini gerçekleştirir ve sonuçları ekranda gösterir
    private static void PerformOCR(string imagePath, string language)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\nİşlem başlatılıyor...");
        Console.WriteLine($"Dosya: {Path.GetFileName(imagePath)}");
        Console.WriteLine($"Dil: {language}");
        Console.ResetColor();

        try
        {
            // Tesseract engine'ini başlat
            using (var engine = new TesseractEngine(tessDataPath, language, EngineMode.Default))
            {
                // Türkçe karakterler için karakter seti tanımla
                engine.SetVariable("tessedit_char_whitelist", "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZabcçdefgğhıijklmnoöprsştuüvyz0123456789.,!?:;-()[]{}\"' ");

                // Görüntüyü yükle ve işle
                using (var img = Pix.LoadFromFile(imagePath))
                {
                    using (var page = engine.Process(img))
                    {
                        string text = page.GetText();
                        float confidence = page.GetMeanConfidence() * 100; // 0-100 aralığına çevir

                        // Metin analizi için istatistikler hesapla
                        int wordCount = CountWords(text);
                        int charCountWithSpaces = text.Length;
                        int charCountWithoutSpaces = text.Replace(" ", "").Replace("\n", "").Replace("\r", "").Length;
                        int lineCount = text.Split('\n', StringSplitOptions.RemoveEmptyEntries).Length;
                        string detectedContent = AnalyzeContent(text);

                        // Sonuçları ekranda göster
                        Console.WriteLine("\n" + new string('=', 50));
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("SONUÇ:");
                        Console.ResetColor();
                        Console.WriteLine(new string('-', 50));

                        if (string.IsNullOrWhiteSpace(text))
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Metin tespit edilemedi.");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine(text.Trim());
                        }

                        Console.WriteLine(new string('-', 50));

                        // Analiz bilgilerini göster
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("ANALİZ:");
                        Console.ResetColor();

                        // Güvenilirlik skorunu renkli olarak göster
                        Console.ForegroundColor = confidence > 80 ? ConsoleColor.Green :
                                                confidence > 60 ? ConsoleColor.Yellow : ConsoleColor.Red;
                        Console.WriteLine($"• Güvenilirlik: {confidence:F1}% {GetConfidenceDescription(confidence)}");
                        Console.ResetColor();

                        // Diğer istatistikleri göster
                        Console.WriteLine($"• Karakter sayısı (boşluksuz): {charCountWithoutSpaces}");
                        Console.WriteLine($"• Karakter sayısı (boşluklu): {charCountWithSpaces}");
                        Console.WriteLine($"• Satır sayısı: {lineCount}");
                        Console.WriteLine($"• İçerik türü: {detectedContent}");

                        Console.WriteLine(new string('=', 50));

                        // Sonuçları dosyaya kaydetme seçeneği sun
                        OfferToSaveResults(text, imagePath, confidence, wordCount, charCountWithSpaces, charCountWithoutSpaces, lineCount, detectedContent);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"İşlem hatası: {ex.Message}");
            Console.ResetColor();
        }
    }

    // Metindeki kelime sayısını hesaplar
    private static int CountWords(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return 0;

        return text.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }

    // Metnin içerik türünü analiz eder (Türkçe, İngilizce, Sayısal vb.)
    private static string AnalyzeContent(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "Belirsiz";

        // Türkçe özel karakterleri say
        int turkishChars = 0;
        char[] turkishSpecialChars = { 'ç', 'ğ', 'ı', 'ö', 'ş', 'ü', 'Ç', 'Ğ', 'İ', 'Ö', 'Ş', 'Ü' };

        foreach (char c in text)
        {
            if (turkishSpecialChars.Contains(c))
                turkishChars++;
        }

        // Sayı karakterlerini say
        int digitCount = text.Count(char.IsDigit);

        // İçerik türünü belirle
        if (digitCount > text.Length * 0.3)
            return "Sayısal İçerik";
        else if (turkishChars > 0)
            return "Türkçe İçerik";
        else
            return "Latin Alfabesi";
    }

    // Güvenilirlik skoruna göre açıklama metni döndürür
    private static string GetConfidenceDescription(float confidence)
    {
        return confidence switch
        {
            > 90 => "(Mükemmel)",
            > 80 => "(İyi)",
            > 60 => "(Orta)",
            > 40 => "(Zayıf)",
            _ => "(Çok Zayıf)"
        };
    }

    // OCR sonuçlarını dosyaya kaydetme seçeneği sunar
    private static void OfferToSaveResults(string text, string imagePath, float confidence, int wordCount, int charCountWithSpaces, int charCountWithoutSpaces, int lineCount, string contentType)
    {
        Console.Write("\nSonuçları dosyaya kaydet? (e/h): ");
        string response = Console.ReadLine()?.ToLower();

        if (response == "e" || response == "evet" || response == "y" || response == "yes")
        {
            try
            {
                // Sonuçlar klasörünü oluştur
                string projectDirectory = Directory.GetCurrentDirectory();
                string resultsFolder = Path.Combine(projectDirectory, "Sonuclar");

                if (!Directory.Exists(resultsFolder))
                {
                    Directory.CreateDirectory(resultsFolder);
                }

                // Dosya adını tarih-saat ile oluştur
                string fileName = $"Sonuc_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                string fullPath = Path.Combine(resultsFolder, fileName);

                // Detaylı rapor içeriği oluştur
                string content = $"=== SONUÇ RAPORU ===\n" +
                               $"Tarih: {DateTime.Now:dd.MM.yyyy HH:mm:ss}\n" +
                               $"Dosya: {Path.GetFileName(imagePath)}\n" +
                               $"Kaynak: {imagePath}\n\n" +
                               $"=== ANALİZ ===\n" +
                               $"Güvenilirlik: {confidence:F1}% {GetConfidenceDescription(confidence)}\n" +
                               $"Karakter sayısı (boşluksuz): {charCountWithoutSpaces}\n" +
                               $"Karakter sayısı (boşluklu): {charCountWithSpaces}\n" +
                               $"Satır Sayısı: {lineCount}\n" +
                               $"İçerik Türü: {contentType}\n\n" +
                               $"=== METİN ===\n" +
                               $"{new string('-', 30)}\n" +
                               $"{text}\n" +
                               $"{new string('-', 30)}";

                // UTF-8 encoding ile dosyaya yaz (Türkçe karakter desteği için)
                File.WriteAllText(fullPath, content, System.Text.Encoding.UTF8);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Kaydedildi: {fullPath}");
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

    // Kullanıcıya başka dosya işlemek isteyip istemediğini sorar
    private static bool AskToContinue()
    {
        Console.Write("\nBaşka dosya işle? (e/h): ");
        string response = Console.ReadLine()?.ToLower();
        return response == "e" || response == "evet" || response == "y" || response == "yes";
    }
}