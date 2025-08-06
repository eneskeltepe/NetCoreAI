# .NET Core AI Projeleri

Bu repo, çeşitli .NET Core teknolojileri ile AI entegrasyonu örneklerini içermektedir. Her proje, farklı bir AI hizmeti veya teknolojisi ile entegrasyon örneği sunmaktadır.

## Projeler

## Project01_ApiDemo
- **Açıklama:** 
  - Basit bir ASP.NET Core Web API projesidir.
  - Müşteri (Customer) CRUD işlemleri (ekle, listele, güncelle, sil) için Entity Framework Core ile SQLite veritabanı kullanır.
  - Temel RESTful servis mantığı ve controller yapısı örneklenmiştir.
- **Gerekli NuGet Paketleri:**
  - `Microsoft.EntityFrameworkCore` (v8.0.17)
  - `Microsoft.EntityFrameworkCore.Design` (v8.0.17)
  - `Microsoft.EntityFrameworkCore.SqlServer` (v8.0.17)
  - `Microsoft.EntityFrameworkCore.Tools` (v8.0.17)
  - `Swashbuckle.AspNetCore` (v8.1.4)

---

## Project02_ApiConsumeUI
- **Açıklama:**
  - ASP.NET Core Razor Pages tabanlı bir web arayüzüdür.
  - Project01'deki API'yi tüketerek müşteri ekleme, listeleme, güncelleme ve silme işlemlerini kullanıcı dostu bir arayüzle sunar.
  - HttpClient ve ViewModel/Dtos kullanımı örneklenmiştir.
- **Gerekli NuGet Paketleri:**
  - `Microsoft.VisualStudio.Web.CodeGeneration.Design` (v8.0.7)
  - `Newtonsoft.Json` (v13.0.3)

---

## Project03_RapidApi
- **Açıklama:**
  - Konsol uygulaması olarak geliştirilmiştir.
  - RapidAPI üzerinden IMDB Top 100 diziler API'sine bağlanır ve dizi listesini ekrana yazdırır.
  - API anahtarı ile dış servis tüketimi ve JSON verisinin modele dönüştürülmesi örneklenmiştir.
- **Gerekli NuGet Paketleri:**
  - `Newtonsoft.Json` (v13.0.3)

---

## Project04_ConsoleAIChat
- **Açıklama:**
  - Konsol uygulaması olarak geliştirilmiştir.
  - Google Gemini API ile ücretsiz olarak sohbet (chatbot) deneyimi sunar.
  - API anahtarı güvenli şekilde appsettings.json dosyasından okunur.
  - Kullanıcı ve yapay zeka mesajları renkli ve geçmişli olarak gösterilir.
  - Microsoft.Extensions.Configuration.Json paketi ile yapılandırma yönetimi örneklenmiştir.
- **Gerekli NuGet Paketleri:**
  - `Microsoft.Extensions.Configuration.Json` (v8.0.1)

---

## Project05_ConsoleSpeechToText
- **Açıklama:**
  - Konsol uygulaması olarak geliştirilmiştir.
  - AssemblyAI API ile ses dosyasını metne çevirir (Speech-to-Text).
  - API anahtarı appsettings.json dosyasından okunur.
  - Kullanıcıdan ses dosyası adı alınabilir, renkli ve adım adım bilgilendirme yapılır.
  - Başarılı ve başarısız durumlar renkli olarak gösterilir, bekleme animasyonu eklenmiştir.
- **Gerekli NuGet Paketleri:**
  - `Microsoft.Extensions.Configuration.Json` (v8.0.1)
- **Örnek Ses Dosyaları:**
  - `audio1.mp3` ve `audio2.mp3` dosyaları test için proje ile birlikte gelir

---

## Project06_ConsoleImageGeneration
- **Açıklama:**
  - Konsol uygulaması olarak geliştirilmiştir.
  - Kullanıcıdan prompt alıp iki farklı model ile görsel üretir:
    - **Stable Diffusion XL (Hugging Face)**: Sadece İngilizce, görseli bilgisayara kaydeder.
    - **FLUX Text To Image (modelslab.com)**: Türkçe/İngilizce, görselin URL'sini döndürür.
  - API anahtarlarını `appsettings.json` dosyasına eklemelisiniz. Örnek için `appsettings.example.json` dosyasına bakabilirsiniz.
- **Gerekli NuGet Paketleri:**
  - `Microsoft.Extensions.Configuration` (v9.0.7)
  - `Microsoft.Extensions.Configuration.Json` (v8.0.1)

---

## Project07_TesseractOcrConsole
- **Açıklama:**
  - Konsol uygulaması olarak geliştirilmiştir.
  - Tesseract OCR ile görüntü dosyalarındaki metinleri okur ve analiz eder.
  - Türkçe ve İngilizce dil desteği, akıllı dosya seçimi ve detaylı analiz raporu sunar.
  - OCR sonuçlarını dosyaya kaydetme özelliği ve konsol içi kurulum rehberi bulunur.
- **Gerekli NuGet Paketleri:**
  - `Tesseract` (v5.2.0)

---

## Project08_GoogleVisionOcrConsole
- **Açıklama:**
  - Konsol uygulaması olarak geliştirilmiştir.
  - Google Cloud Vision API ile resimlerden metin çıkarma (OCR) işlemi yapar.
  - Images klasöründeki resimleri otomatik listeler ve seçim yapmayı kolaylaştırır.
  - OCR sonuçlarını dosyaya kaydetme özelliği bulunur.
  - Google Cloud Service Account JSON dosyası gereklidir.
- **Gerekli NuGet Paketleri:**
  - `Google.Cloud.Vision.V1` (v3.14.0)

---

## Project09_TranslatorAIConsole
- **Açıklama:**
  - Konsol uygulaması olarak geliştirilmiştir.
  - Google AI Studio (Gemini) API ile çok dilli çeviri işlemleri yapar.
  - 7 farklı dil desteği (Türkçe, İngilizce, Almanca, Fransızca, İspanyolca, İtalyanca, Portekizce) bulunur.
  - Hızlı çeviri özelliği ile sadece Türkçe'den İngilizce'ye çeviri yapar.
  - Özel dil seçimi ile kullanıcı kaynak ve hedef dili manuel olarak belirleyebilir.
  - Sürekli çeviri döngüsü ile 'q' yazılana kadar çeviri yapmaya devam eder.
  - API anahtarı güvenli şekilde appsettings.json dosyasından okunur.
- **Gerekli NuGet Paketleri:**
  - `Microsoft.Extensions.Configuration.Json` (v8.0.1)
  - `System.Text.Json` (v8.0.6)

---

## Project10_TextToSpeechConsole
- **Açıklama:**
  - Konsol uygulaması olarak geliştirilmiştir.
  - System.Speech.Synthesis kullanarak metni sese çevirir (Text-to-Speech).
  - Gelişmiş kullanıcı arayüzü ile ses seçimi, hız ve volüm ayarları bulunur.
  - Varsayılan ayarlarla hızlı başlangıç, isteğe bağlı detaylı ayar menüsü sunar.
  - Mevcut sistemdeki tüm TTS seslerini listeler ve seçim yapma imkanı verir.
  - Türkçe ses desteği (Windows'ta Türkçe TTS sesi varsa otomatik algılar).
  - Sürekli metin girme döngüsü ile çoklu kullanım olanağı sağlar.
- **Gerekli NuGet Paketleri:**
  - `System.Speech` (Windows built-in)

---

## Project11_ConsoleTextToSpeechWithGoogleCloudAPI
- **Açıklama:**
  - Konsol uygulaması olarak geliştirilmiştir.
  - Google Cloud Text-to-Speech API ile profesyonel metinden sese çevirme işlemleri yapar.
  - 7 farklı dil desteği (Türkçe, İngilizce (ABD/İngiltere), Almanca, Fransızca, İspanyolca, İtalyanca) bulunur.
  - **Özellikler:**
    - **Hızlı Seslendirme**: Varsayılan Türkçe kadın sesi ile basit TTS
    - **Gelişmiş Seslendirme**: Dil, ses, hız, ton ve format seçimi ile özelleştirilebilir TTS
    - **Batch İşleme**: Çoklu metni aynı anda işleyerek ayrı dosyalar oluşturma
    - **Mevcut Sesler**: Google Cloud'daki tüm mevcut sesleri listeleme
    - **Dosyadan Okuma**: .txt dosyalarından metin okuyarak seslendirme
  - Konuşma hızı (0.5x - 1.5x), ses tonu (-5.0 ile +5.0 arası) ve format (MP3/WAV) seçenekleri mevcuttur.
  - Google Cloud Service Account JSON dosyası gereklidir (`google-cloud-credentials-example.json` dosyasına bakın).
- **Gerekli NuGet Paketleri:**
  - `Google.Cloud.TextToSpeech.V1` (v3.12.0)

---

## Project12_SentimentAIAppConsole
- **Açıklama:**
  - Konsol uygulaması olarak geliştirilmiştir.
  - Google AI Studio (Gemini) API ile metin sentiment analizi yapar.
  - Metinleri "Pozitif", "Negatif" veya "Nötr" kategorilerine ayırır.
  - Renkli konsol çıktıları ile sentiment sonuçlarını gösterir (Pozitif: Yeşil, Negatif: Kırmızı, Nötr: Sarı).
  - Döngülü uygulama ile çıkış yapılana kadar sürekli metin analiz edebilme özelliği bulunur.
  - Türkçe metinleri doğru şekilde analiz eder ve akıllı normalizasyon yapar.
  - API anahtarı güvenli şekilde appsettings.json dosyasından okunur.
- **Gerekli NuGet Paketleri:**
  - `Microsoft.Extensions.Configuration` (v8.0.0)
  - `Microsoft.Extensions.Configuration.Json` (v8.0.0)

---

## Project13_SentimentWithDegreeAIAppConsole
- **Açıklama:**
  - Konsol uygulaması olarak geliştirilmiştir.
  - Google AI Studio (Gemini) API ile gelişmiş metin sentiment analizi yapar.
  - 6 farklı duygu kategorisi için detaylı analiz: Mutluluk, Üzüntü, Öfke, Korku, Şaşırma, Tarafsız.
  - Her duygu için 0-100% arası skorlar ve görsel progress bar gösterir.
  - Renkli konsol çıktıları ile her duygu için özel renk kodlaması (Mutluluk: Yeşil, Üzüntü: Mavi, Öfke: Kırmızı vb.).
  - JSON formatında yapılandırılmış sonuçlar döndürür.
  - Döngülü uygulama ile çıkış yapılana kadar sürekli metin analiz edebilme özelliği bulunur.
  - API anahtarı güvenli şekilde appsettings.json dosyasından okunur.
- **Gerekli NuGet Paketleri:**
  - `Microsoft.Extensions.Configuration` (v8.0.0)
  - `Microsoft.Extensions.Configuration.Json` (v8.0.0)

---

## Project14_TextSummarizerAIWeb
- **Açıklama:**
  - ASP.NET Core Razor Pages tabanlı bir web uygulamasıdır.
  - Google AI Studio (Gemini 2.5-flash) API ile metinleri özetler ve analiz eder.
  - Modern glassmorphism tasarımı ile kullanıcı dostu arayüz sunar.
  - **Ana Özellikler:**
    - **Metin Özetleme**: Gemini API ile akıllı metin özetleme
    - **Çoklu Dosya Desteği**: TXT, PDF, DOCX dosyalarından metin okuma
    - **Geçmiş Yönetimi**: localStorage tabanlı özet geçmişi sistemi
    - **Çoklu İndirme**: HTML, TXT, Markdown ve DOCX formatlarında export
    - **Özelleştirilmiş UI**: Bootstrap 5 ve glassmorphism efektli modern tasarım
    - **Benzersiz ID Sistemi**: Her özet için kalıcı ve eşsiz kimlik
    - **Yükleme Animasyonu**: Özetleme sırasında görsel geri bildirim
  - Türkçe ve İngilizce dil desteği ile çok dilli içerik analiz edebilir.
  - Responsive tasarım ile mobil ve masaüstü uyumludur.
  - API anahtarı güvenli şekilde appsettings.json dosyasından okunur.
- **Gerekli NuGet Paketleri:**
  - `DocumentFormat.OpenXml` (v3.2.0) - DOCX dosya işleme
  - `iTextSharp` (v5.5.13.4) - PDF dosya okuma
  - `System.Text.Json` (v8.0.6) - JSON işleme

---

Her proje kendi klasöründe, ilgili kod ve yapılandırma dosyalarını içermektedir. Yeni projeler eklendikçe bu README dosyası güncellenecektir.