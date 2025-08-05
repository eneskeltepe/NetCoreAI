using Google.Cloud.TextToSpeech.V1;
using System.Globalization;

class Program
{
    // Google Cloud Text-to-Speech client'ını tutar (static olarak tanımlandı çünkü tüm metotlarda kullanılacak)
    private static TextToSpeechClient? client;

    // Google Cloud kimlik bilgileri dosyasının yolu
    private static readonly string credentialsPath = "famous-modem-465613-k1-e34190f6e788.json";

    static async Task Main(string[] args)
    {
        // Konsol penceresinin başlığını ayarla
        Console.Title = "Google Cloud Text-to-Speech Uygulaması";

        // Türkçe karakterlerin doğru görünmesi için UTF-8 encoding ayarla
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        try
        {
            // Google Cloud kimlik bilgilerini ayarla ve kontrol et
            if (!SetupGoogleCredentials())
            {
                Console.WriteLine("Uygulama kapatılıyor...");
                Console.ReadKey();
                return;
            }

            // Text-to-Speech client'ını başlat
            client = TextToSpeechClient.Create();

            // Hoş geldin mesajını göster
            ShowWelcomeMessage();

            // Ana uygulama döngüsü - kullanıcı çıkana kadar devam eder
            bool continueApp = true;
            while (continueApp)
            {
                ShowMainMenu();
                string choice = Console.ReadLine()?.Trim();

                // Kullanıcının seçimine göre ilgili fonksiyonu çalıştır
                switch (choice)
                {
                    case "1":
                        await QuickTextToSpeech(); // Hızlı seslendirme
                        break;
                    case "2":
                        await AdvancedTextToSpeech(); // Gelişmiş seslendirme
                        break;
                    case "3":
                        await BatchTextToSpeech(); // Toplu işleme
                        break;
                    case "4":
                        await ShowAvailableVoices(); // Mevcut sesleri listele
                        break;
                    case "5":
                        await TextFromFile(); // Dosyadan metin okuma
                        break;
                    case "6":
                    case "q":
                    case "quit":
                    case "çık":
                        continueApp = false; // Uygulamadan çık
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Geçersiz seçim! Lütfen 1-6 arasında bir seçim yapın.\n");
                        Console.ResetColor();
                        break;
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Uygulama kapatılıyor. İyi günler!");
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

    /// <summary>
    /// Google Cloud kimlik bilgilerini sistem ortam değişkenine ayarlar
    /// </summary>
    private static bool SetupGoogleCredentials()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Google Cloud credentials dosyası kontrol ediliyor...");
        Console.ResetColor();

        // Credentials dosyasının varlığını kontrol et
        if (!File.Exists(credentialsPath))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"HATA: Credentials dosyası bulunamadı: {credentialsPath}");
            Console.WriteLine("Lütfen Google Cloud Service Account JSON dosyasını proje klasörüne koyun.");
            Console.ResetColor();
            return false;
        }

        // Google Cloud kimlik bilgilerini sistem ortam değişkenine ata
        // Bu, Google Cloud SDK'nın kimlik doğrulaması için gereklidir
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Google Cloud credentials başarıyla ayarlandı!");
        Console.ResetColor();
        return true;
    }

    /// <summary>
    /// Uygulama başlatıldığında gösterilen hoş geldin mesajı
    /// </summary>
    private static void ShowWelcomeMessage()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                   Google Cloud Text-to-Speech                ║");
        Console.WriteLine("║                   Gelişmiş Konsol Uygulaması                 ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
        Console.WriteLine();
        Console.ResetColor();

        // Uygulamanın özelliklerini listele
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("* Çoklu dil desteği");
        Console.WriteLine("* Farklı ses tipleri ve tonları");
        Console.WriteLine("* Özelleştirilebilir ses hızı ve pitch");
        Console.WriteLine("* Batch işleme desteği");
        Console.WriteLine("* Dosyadan metin okuma");
        Console.WriteLine("* Çoklu format desteği (MP3, WAV)");
        Console.ResetColor();
        Console.WriteLine();
    }

    /// <summary>
    /// Ana menüyü ekranda gösterir
    /// </summary>
    private static void ShowMainMenu()
    {
        Console.WriteLine(new string('=', 60));
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("ANA MENÜ");
        Console.ResetColor();
        Console.WriteLine("1. Hızlı Metin Seslendirme");
        Console.WriteLine("2. Gelişmiş Seslendirme (Özelleştirilebilir)");
        Console.WriteLine("3. Batch İşleme (Çoklu Metin)");
        Console.WriteLine("4. Mevcut Sesleri Görüntüle");
        Console.WriteLine("5. Dosyadan Metin Okuma");
        Console.WriteLine("6. Çıkış");
        Console.WriteLine(new string('=', 60));
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Seçiminizi yapın (1-6): ");
        Console.ResetColor();
    }

    /// <summary>
    /// Hızlı metin seslendirme - Varsayılan ayarlarla basit TTS işlemi
    /// </summary>
    private static async Task QuickTextToSpeech()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== HIZLI METİN SESLENDİRME ===\n");
        Console.ResetColor();

        Console.Write("Seslendirilecek metni girin: ");
        string text = Console.ReadLine();

        // Boş metin kontrolü
        if (string.IsNullOrWhiteSpace(text))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Geçerli bir metin girin!");
            Console.ResetColor();
            WaitForKeyPress();
            return;
        }

        try
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Ses dosyası oluşturuluyor...");
            Console.ResetColor();

            // Metin girişini hazırla
            var input = new SynthesisInput { Text = text };

            // Ses parametrelerini ayarla (Türkçe kadın sesi varsayılan)
            var voice = new VoiceSelectionParams
            {
                LanguageCode = "tr-TR",
                Name = "tr-TR-Standard-A",
                SsmlGender = SsmlVoiceGender.Female
            };

            // Ses dosyası formatını ayarla (MP3 formatı)
            var config = new AudioConfig { AudioEncoding = AudioEncoding.Mp3 };

            // Google Cloud API'sine istek gönder
            var response = await client.SynthesizeSpeechAsync(input, voice, config);

            // Dosya adını tarih-saat ile benzersiz yap
            string fileName = $"output_{DateTime.Now:yyyyMMdd_HHmmss}.mp3";

            // Ses dosyasını diske kaydet
            await File.WriteAllBytesAsync(fileName, response.AudioContent.ToByteArray());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"* Ses dosyası başarıyla oluşturuldu: {fileName}");
            Console.ResetColor();

            // Kullanıcıya dosyayı açmak isteyip istemediğini sor
            Console.Write("Dosyayı şimdi açmak ister misiniz? (E/H): ");
            string playChoice = Console.ReadLine()?.ToUpper();
            if (playChoice == "E" || playChoice == "EVET")
            {
                OpenAudioFile(fileName);
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Hata oluştu: {ex.Message}");
            Console.ResetColor();
        }

        WaitForKeyPress();
    }

    /// <summary>
    /// Gelişmiş seslendirme - Kullanıcının tüm parametreleri özelleştirebildiği mod
    /// </summary>
    private static async Task AdvancedTextToSpeech()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== GELİŞMİŞ SESLENDİRME ===\n");
        Console.ResetColor();

        Console.Write("Seslendirilecek metni girin: ");
        string text = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(text))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Geçerli bir metin girin!");
            Console.ResetColor();
            WaitForKeyPress();
            return;
        }

        // Kullanıcıdan sırasıyla seçimleri al
        var language = SelectLanguage();        // Dil seçimi
        var voice = await SelectVoice(language);  // Ses seçimi  
        var speakingRate = SelectSpeakingRate(); // Konuşma hızı
        var pitch = SelectPitch();              // Ses tonu
        var audioFormat = SelectAudioFormat();  // Dosya formatı

        try
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Gelişmiş ses dosyası oluşturuluyor...");
            Console.ResetColor();

            // Metin girişini hazırla
            var input = new SynthesisInput { Text = text };

            // Ses konfigürasyonunu kullanıcının seçimlerine göre ayarla
            var config = new AudioConfig
            {
                AudioEncoding = audioFormat,
                SpeakingRate = speakingRate,  // Konuşma hızı (0.25 ile 4.0 arası)
                Pitch = pitch                 // Ses tonu (-20.0 ile +20.0 arası)
            };

            // Google Cloud API'sine istek gönder
            var response = await client.SynthesizeSpeechAsync(input, voice, config);

            // Dosya uzantısını formata göre belirle
            string extension = audioFormat == AudioEncoding.Mp3 ? "mp3" : "wav";
            string fileName = $"advanced_output_{DateTime.Now:yyyyMMdd_HHmmss}.{extension}";

            // Ses dosyasını kaydet
            await File.WriteAllBytesAsync(fileName, response.AudioContent.ToByteArray());

            // Başarı mesajı ve dosya bilgilerini göster
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"* Gelişmiş ses dosyası başarıyla oluşturuldu: {fileName}");
            Console.WriteLine($"  - Dil: {language}");
            Console.WriteLine($"  - Ses: {voice.Name}");
            Console.WriteLine($"  - Hız: {speakingRate:F1}x");
            Console.WriteLine($"  - Ton: {pitch:F1}");
            Console.WriteLine($"  - Format: {extension.ToUpper()}");
            Console.ResetColor();

            Console.Write("Dosyayı şimdi açmak ister misiniz? (E/H): ");
            string playChoice = Console.ReadLine()?.ToUpper();
            if (playChoice == "E" || playChoice == "EVET")
            {
                OpenAudioFile(fileName);
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Hata oluştu: {ex.Message}");
            Console.ResetColor();
        }

        WaitForKeyPress();
    }

    /// <summary>
    /// Batch işleme - Birden fazla metni aynı anda işleyerek ayrı dosyalar oluşturur
    /// </summary>
    private static async Task BatchTextToSpeech()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== BATCH İŞLEME (ÇOKLU METİN) ===\n");
        Console.ResetColor();

        // Metinleri saklamak için liste oluştur
        var texts = new List<string>();

        Console.WriteLine("Metinleri girin (boş satır için Enter'a basın, bitirmek için 'TAMAM' yazın):");
        Console.WriteLine();

        // Kullanıcıdan metinleri tek tek al
        int counter = 1;
        while (true)
        {
            Console.Write($"{counter}. Metin: ");
            string input = Console.ReadLine();

            // Boş satır kontrolü
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Boş satır atlandı.");
                continue;
            }

            // Bitirme komutu kontrolü
            if (input.ToUpper() == "TAMAM")
                break;

            texts.Add(input);
            counter++;
        }

        // En az bir metin girilmiş mi kontrol et
        if (texts.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Hiç metin girilmedi!");
            Console.ResetColor();
            WaitForKeyPress();
            return;
        }

        // Tüm metinler için aynı ses ayarlarını kullan
        var language = SelectLanguage();
        var voice = await SelectVoice(language);

        try
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Batch işleme başlatılıyor... ({texts.Count} metin)");
            Console.ResetColor();

            // Batch sonuçları için klasör oluştur
            string batchFolder = $"batch_{DateTime.Now:yyyyMMdd_HHmmss}";
            Directory.CreateDirectory(batchFolder);

            // Her metni sırasıyla işle
            for (int i = 0; i < texts.Count; i++)
            {
                Console.WriteLine($"İşleniyor: {i + 1}/{texts.Count}");

                // Metin girişini hazırla
                var input = new SynthesisInput { Text = texts[i] };
                var config = new AudioConfig { AudioEncoding = AudioEncoding.Mp3 };

                // API'ye istek gönder
                var response = await client.SynthesizeSpeechAsync(input, voice, config);

                // Dosya adını sıralı olarak oluştur (001, 002, 003...)
                string fileName = Path.Combine(batchFolder, $"batch_{i + 1:D3}.mp3");
                await File.WriteAllBytesAsync(fileName, response.AudioContent.ToByteArray());
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"* Batch işleme tamamlandı! Dosyalar '{batchFolder}' klasöründe");
            Console.ResetColor();

            // Klasörü açmak isteyip istemediğini sor
            Console.Write("Klasörü şimdi açmak ister misiniz? (E/H): ");
            string openChoice = Console.ReadLine()?.ToUpper();
            if (openChoice == "E" || openChoice == "EVET")
            {
                System.Diagnostics.Process.Start("explorer.exe", batchFolder);
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Hata oluştu: {ex.Message}");
            Console.ResetColor();
        }

        WaitForKeyPress();
    }

    /// <summary>
    /// Google Cloud'da mevcut olan tüm sesleri listeler
    /// </summary>
    private static async Task ShowAvailableVoices()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== MEVCUT SESLER ===\n");
        Console.ResetColor();

        try
        {
            Console.WriteLine("Mevcut sesler yükleniyor...");

            // Google Cloud'dan tüm mevcut sesleri al
            var voices = await client.ListVoicesAsync(new ListVoicesRequest());

            // Sesleri dillere göre grupla
            var groupedVoices = voices.Voices
                .GroupBy(v => v.LanguageCodes.FirstOrDefault())
                .OrderBy(g => g.Key);

            // Her dil grubu için sesleri listele
            foreach (var group in groupedVoices)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n[Dil] {GetLanguageName(group.Key)} ({group.Key})");
                Console.ResetColor();

                // Dil grubundaki her sesi listele
                foreach (var voice in group.OrderBy(v => v.Name))
                {
                    // Ses cinsiyetini Türkçe'ye çevir
                    string gender = voice.SsmlGender switch
                    {
                        SsmlVoiceGender.Male => "Erkek",
                        SsmlVoiceGender.Female => "Kadın",
                        _ => "Belirsiz"
                    };

                    Console.WriteLine($"  * {voice.Name} ({gender})");
                }
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Hata oluştu: {ex.Message}");
            Console.ResetColor();
        }

        WaitForKeyPress();
    }

    /// <summary>
    /// Dosyadan metin okuyarak seslendirme yapar
    /// </summary>
    private static async Task TextFromFile()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== DOSYADAN METİN OKUMA ===\n");
        Console.ResetColor();

        Console.Write("Metin dosyasının yolunu girin (txt dosyası): ");
        string filePath = Console.ReadLine()?.Trim();

        // Dosya varlığını kontrol et
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Geçerli bir dosya yolu girin!");
            Console.ResetColor();
            WaitForKeyPress();
            return;
        }

        try
        {
            // Dosyayı UTF-8 encoding ile oku (Türkçe karakter desteği için)
            string text = await File.ReadAllTextAsync(filePath);

            // Dosya boş mu kontrol et
            if (string.IsNullOrWhiteSpace(text))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Dosya boş!");
                Console.ResetColor();
                WaitForKeyPress();
                return;
            }

            // Dosya içeriği hakkında bilgi ver
            Console.WriteLine($"Dosya içeriği okundu ({text.Length} karakter)");
            Console.WriteLine("İlk 100 karakter:");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(text.Length > 100 ? text.Substring(0, 100) + "..." : text);
            Console.ResetColor();

            // Ses ayarlarını kullanıcıdan al
            var language = SelectLanguage();
            var voice = await SelectVoice(language);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Dosyadan ses oluşturuluyor...");
            Console.ResetColor();

            // Metin girişini hazırla
            var input = new SynthesisInput { Text = text };
            var config = new AudioConfig { AudioEncoding = AudioEncoding.Mp3 };

            // API'ye istek gönder
            var response = await client.SynthesizeSpeechAsync(input, voice, config);

            // Dosya adını "file_output_" ön eki ile oluştur
            string fileName = $"file_output_{DateTime.Now:yyyyMMdd_HHmmss}.mp3";
            await File.WriteAllBytesAsync(fileName, response.AudioContent.ToByteArray());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"* Dosyadan ses başarıyla oluşturuldu: {fileName}");
            Console.ResetColor();

            Console.Write("Dosyayı şimdi açmak ister misiniz? (E/H): ");
            string playChoice = Console.ReadLine()?.ToUpper();
            if (playChoice == "E" || playChoice == "EVET")
            {
                OpenAudioFile(fileName);
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Hata oluştu: {ex.Message}");
            Console.ResetColor();
        }

        WaitForKeyPress();
    }

    /// <summary>
    /// Kullanıcıdan dil seçimi alır
    /// </summary>
    private static string SelectLanguage()
    {
        Console.WriteLine("\nDil seçin:");
        Console.WriteLine("1. Türkçe (tr-TR)");
        Console.WriteLine("2. İngilizce (en-US)");
        Console.WriteLine("3. İngilizce İngiliz (en-GB)");
        Console.WriteLine("4. Almanca (de-DE)");
        Console.WriteLine("5. Fransızca (fr-FR)");
        Console.WriteLine("6. İspanyolca (es-ES)");
        Console.WriteLine("7. İtalyanca (it-IT)");

        Console.Write("Seçiminiz (1-7): ");
        string choice = Console.ReadLine();

        // Kullanıcının seçimine göre dil kodunu döndür
        return choice switch
        {
            "1" => "tr-TR",
            "2" => "en-US",
            "3" => "en-GB",
            "4" => "de-DE",
            "5" => "fr-FR",
            "6" => "es-ES",
            "7" => "it-IT",
            _ => "tr-TR" // Geçersiz seçim için varsayılan olarak Türkçe
        };
    }

    /// <summary>
    /// Seçilen dil için mevcut sesleri listeler ve kullanıcıdan seçim alır
    /// </summary>
    private static async Task<VoiceSelectionParams> SelectVoice(string languageCode)
    {
        try
        {
            // Belirtilen dil için mevcut sesleri Google Cloud'dan al
            var voices = await client.ListVoicesAsync(new ListVoicesRequest { LanguageCode = languageCode });
            var availableVoices = voices.Voices.Where(v => v.LanguageCodes.Contains(languageCode)).ToList();

            // Eğer ses bulunamazsa varsayılan döndür
            if (availableVoices.Count == 0)
            {
                return new VoiceSelectionParams
                {
                    LanguageCode = languageCode,
                    SsmlGender = SsmlVoiceGender.Female
                };
            }

            // Mevcut sesleri listele (maksimum 10 tane)
            Console.WriteLine($"\n{GetLanguageName(languageCode)} için mevcut sesler:");
            for (int i = 0; i < availableVoices.Count && i < 10; i++)
            {
                string gender = availableVoices[i].SsmlGender switch
                {
                    SsmlVoiceGender.Male => "Erkek",
                    SsmlVoiceGender.Female => "Kadın",
                    _ => "Belirsiz"
                };
                Console.WriteLine($"{i + 1}. {availableVoices[i].Name} ({gender})");
            }

            Console.Write($"Ses seçin (1-{Math.Min(availableVoices.Count, 10)}): ");
            string choice = Console.ReadLine();

            // Kullanıcının seçimini kontrol et ve ilgili sesi döndür
            if (int.TryParse(choice, out int index) && index > 0 && index <= availableVoices.Count)
            {
                var selectedVoice = availableVoices[index - 1];
                return new VoiceSelectionParams
                {
                    LanguageCode = languageCode,
                    Name = selectedVoice.Name,
                    SsmlGender = selectedVoice.SsmlGender
                };
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Ses listesi alınamadı: {ex.Message}");
            Console.ResetColor();
        }

        // Hata durumunda veya geçersiz seçimde varsayılan ses döndür
        return new VoiceSelectionParams
        {
            LanguageCode = languageCode,
            SsmlGender = SsmlVoiceGender.Female
        };
    }

    /// <summary>
    /// Kullanıcıdan konuşma hızı seçimi alır
    /// </summary>
    private static double SelectSpeakingRate()
    {
        Console.WriteLine("\nKonuşma hızı seçin:");
        Console.WriteLine("1. Çok Yavaş (0.5x)");
        Console.WriteLine("2. Yavaş (0.75x)");
        Console.WriteLine("3. Normal (1.0x)");
        Console.WriteLine("4. Hızlı (1.25x)");
        Console.WriteLine("5. Çok Hızlı (1.5x)");

        Console.Write("Seçiminiz (1-5): ");
        string choice = Console.ReadLine();

        // Kullanıcının seçimine göre hız değerini döndür
        return choice switch
        {
            "1" => 0.5,   // Yarı hız
            "2" => 0.75,  // Yavaş
            "3" => 1.0,   // Normal hız
            "4" => 1.25,  // Hızlı
            "5" => 1.5,   // Çok hızlı
            _ => 1.0      // Varsayılan normal hız
        };
    }

    /// <summary>
    /// Kullanıcıdan ses tonu (pitch) seçimi alır
    /// </summary>
    private static double SelectPitch()
    {
        Console.WriteLine("\nSes tonu seçin:");
        Console.WriteLine("1. Çok Alçak (-5.0)");
        Console.WriteLine("2. Alçak (-2.0)");
        Console.WriteLine("3. Normal (0.0)");
        Console.WriteLine("4. Yüksek (2.0)");
        Console.WriteLine("5. Çok Yüksek (5.0)");

        Console.Write("Seçiminiz (1-5): ");
        string choice = Console.ReadLine();

        // Kullanıcının seçimine göre ton değerini döndür
        return choice switch
        {
            "1" => -5.0,  // Çok alçak ton
            "2" => -2.0,  // Alçak ton
            "3" => 0.0,   // Normal ton
            "4" => 2.0,   // Yüksek ton
            "5" => 5.0,   // Çok yüksek ton
            _ => 0.0      // Varsayılan normal ton
        };
    }

    /// <summary>
    /// Kullanıcıdan ses dosyası formatı seçimi alır
    /// </summary>
    private static AudioEncoding SelectAudioFormat()
    {
        Console.WriteLine("\nSes formatı seçin:");
        Console.WriteLine("1. MP3 (Önerilen)");
        Console.WriteLine("2. WAV (Yüksek kalite)");

        Console.Write("Seçiminiz (1-2): ");
        string choice = Console.ReadLine();

        // Kullanıcının seçimine göre format döndür
        return choice switch
        {
            "1" => AudioEncoding.Mp3,      // MP3 formatı (küçük dosya boyutu)
            "2" => AudioEncoding.Linear16, // WAV formatı (yüksek kalite)
            _ => AudioEncoding.Mp3         // Varsayılan MP3
        };
    }

    /// <summary>
    /// Dil kodunu Türkçe isime çevirir
    /// </summary>
    private static string GetLanguageName(string languageCode)
    {
        return languageCode switch
        {
            "tr-TR" => "Türkçe",
            "en-US" => "İngilizce (ABD)",
            "en-GB" => "İngilizce (İngiltere)",
            "de-DE" => "Almanca",
            "fr-FR" => "Fransızca",
            "es-ES" => "İspanyolca",
            "it-IT" => "İtalyanca",
            _ => languageCode // Bilinmeyen dil için kod'u aynen döndür
        };
    }

    /// <summary>
    /// Ses dosyasını varsayılan uygulama ile açar
    /// </summary>
    private static void OpenAudioFile(string fileName)
    {
        try
        {
            // Windows'ta varsayılan medya oynatıcıyla dosyayı aç
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = fileName,
                UseShellExecute = true // Sistem varsayılan uygulamasını kullan
            });
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Dosya açılamadı: {ex.Message}");
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Kullanıcıdan herhangi bir tuşa basmasını bekler
    /// </summary>
    private static void WaitForKeyPress()
    {
        Console.WriteLine("\nDevam etmek için herhangi bir tuşa basın...");
        Console.ReadKey();
    }
}