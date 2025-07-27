using System.Speech.Synthesis;

class Program
{
    static void Main(string[] args)
    {
        Console.Title = "Text-to-Speech Uygulaması";
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("========================================");
        Console.WriteLine("  Text-to-Speech Uygulaması");
        Console.WriteLine("========================================\n");
        Console.ResetColor();

        SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();

        // Varsayılan ayarlar
        speechSynthesizer.Rate = 0;     // Normal hız
        speechSynthesizer.Volume = 80;  // %80 ses seviyesi

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Sistem hazır! Varsayılan ayarlar:");
        Console.WriteLine($"- Ses hızı: Normal (0)");
        Console.WriteLine($"- Ses seviyesi: %80");
        Console.ResetColor();

        // Ana döngü
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nMetni Girin (ayarlar için 'a', çıkmak için 'q'): ");
            Console.ResetColor();
            string input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Lütfen geçerli bir metin girin.");
                Console.ResetColor();
                continue;
            }

            if (input.ToLower() == "q" || input.ToLower() == "quit" || input.ToLower() == "çık")
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Programa son veriliyor...");
                Console.ResetColor();
                break;
            }

            if (input.ToLower() == "a" || input.ToLower() == "ayarlar")
            {
                ShowSettingsMenu(speechSynthesizer);
                continue;
            }

            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Konuşuyor...");
                Console.ResetColor();
                
                speechSynthesizer.Speak(input);
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Tamamlandı!");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                Console.ResetColor();
            }
        }

        speechSynthesizer.Dispose();
    }

    static void ShowSettingsMenu(SpeechSynthesizer speechSynthesizer)
    {
        while (true)
        {
            Console.WriteLine("\n" + new string('=', 40));
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("AYARLAR MENÜSÜ");
            Console.ResetColor();
            Console.WriteLine("1. Ses listesini göster ve seç");
            Console.WriteLine("2. Konuşma hızını değiştir");
            Console.WriteLine("3. Ses seviyesini değiştir");
            Console.WriteLine("4. Mevcut ayarları göster");
            Console.WriteLine("5. Ana menüye dön");

            Console.Write("\nSeçiminizi yapın (1-5): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ChangeVoice(speechSynthesizer);
                    break;
                case "2":
                    ChangeRate(speechSynthesizer);
                    break;
                case "3":
                    ChangeVolume(speechSynthesizer);
                    break;
                case "4":
                    ShowCurrentSettings(speechSynthesizer);
                    break;
                case "5":
                    return;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Geçersiz seçim! Lütfen 1-5 arası bir sayı girin.");
                    Console.ResetColor();
                    break;
            }
        }
    }

    static void ChangeVoice(SpeechSynthesizer speechSynthesizer)
    {
        var voices = speechSynthesizer.GetInstalledVoices();
        Console.WriteLine("\nMevcut Sesler:");
        Console.WriteLine("==============");

        for (int i = 0; i < voices.Count; i++)
        {
            var voice = voices[i].VoiceInfo;
            string cultureDisplay = voice.Culture.DisplayName;
            string genderDisplay = voice.Gender.ToString();
            Console.WriteLine($"{i + 1}. {voice.Name} ({cultureDisplay}) - {genderDisplay}");
        }

        Console.Write($"\nHangi sesi kullanmak istiyorsunuz? (1-{voices.Count}): ");
        string voiceChoice = Console.ReadLine();
        
        if (!string.IsNullOrEmpty(voiceChoice) && int.TryParse(voiceChoice, out int choice) && choice >= 1 && choice <= voices.Count)
        {
            speechSynthesizer.SelectVoice(voices[choice - 1].VoiceInfo.Name);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Ses değiştirildi: {voices[choice - 1].VoiceInfo.Name}");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Geçersiz seçim!");
            Console.ResetColor();
        }
    }

    static void ChangeRate(SpeechSynthesizer speechSynthesizer)
    {
        Console.WriteLine("\nKonuşma Hızı Ayarı:");
        Console.WriteLine("====================");
        Console.WriteLine("-10: En yavaş");
        Console.WriteLine("  0: Normal (varsayılan)");
        Console.WriteLine("+10: En hızlı");
        
        Console.Write($"\nYeni hız değeri (-10 ile +10 arası) [Şu anki: {speechSynthesizer.Rate}]: ");
        string rateInput = Console.ReadLine();
        
        if (!string.IsNullOrEmpty(rateInput) && int.TryParse(rateInput, out int rate) && rate >= -10 && rate <= 10)
        {
            speechSynthesizer.Rate = rate;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Hız değiştirildi: {rate}");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Geçersiz değer! -10 ile +10 arası bir sayı girin.");
            Console.ResetColor();
        }
    }

    static void ChangeVolume(SpeechSynthesizer speechSynthesizer)
    {
        Console.WriteLine("\nSes Seviyesi Ayarı:");
        Console.WriteLine("===================");
        Console.WriteLine("  0: Sessiz");
        Console.WriteLine(" 50: Orta");
        Console.WriteLine("100: Maksimum");
        
        Console.Write($"\nYeni ses seviyesi (0-100 arası) [Şu anki: {speechSynthesizer.Volume}]: ");
        string volumeInput = Console.ReadLine();
        
        if (!string.IsNullOrEmpty(volumeInput) && int.TryParse(volumeInput, out int volume) && volume >= 0 && volume <= 100)
        {
            speechSynthesizer.Volume = volume;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Ses seviyesi değiştirildi: %{volume}");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Geçersiz değer! 0 ile 100 arası bir sayı girin.");
            Console.ResetColor();
        }
    }

    static void ShowCurrentSettings(SpeechSynthesizer speechSynthesizer)
    {
        Console.WriteLine("\nMevcut Ayarlar:");
        Console.WriteLine("===============");
        Console.WriteLine($"Ses: {speechSynthesizer.Voice.Name}");
        Console.WriteLine($"Dil: {speechSynthesizer.Voice.Culture.DisplayName}");
        Console.WriteLine($"Cinsiyet: {speechSynthesizer.Voice.Gender}");
        Console.WriteLine($"Hız: {speechSynthesizer.Rate}");
        Console.WriteLine($"Ses Seviyesi: %{speechSynthesizer.Volume}");
    }
}