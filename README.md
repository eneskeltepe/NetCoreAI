# .NET Core AI Projeleri

Bu repo, çeþitli .NET Core teknolojileri ile AI entegrasyonu örneklerini içermektedir. Her proje, farklý bir AI hizmeti veya teknolojisi ile entegrasyon örneði sunmaktadýr.

## Projeler

## Project01_ApiDemo
- **Açýklama:** 
  - Basit bir ASP.NET Core Web API projesidir.
  - Müþteri (Customer) CRUD iþlemleri (ekle, listele, güncelle, sil) için Entity Framework Core ile SQLite veritabaný kullanýr.
  - Temel RESTful servis mantýðý ve controller yapýsý örneklenmiþtir.
- **Gerekli NuGet Paketleri:**
  - `Microsoft.EntityFrameworkCore` (v8.0.17)
  - `Microsoft.EntityFrameworkCore.Design` (v8.0.17)
  - `Microsoft.EntityFrameworkCore.SqlServer` (v8.0.17)
  - `Microsoft.EntityFrameworkCore.Tools` (v8.0.17)
  - `Swashbuckle.AspNetCore` (v8.1.4)

---

## Project02_ApiConsumeUI
- **Açýklama:**
  - ASP.NET Core Razor Pages tabanlý bir web arayüzüdür.
  - Project01'deki API'yi tüketerek müþteri ekleme, listeleme, güncelleme ve silme iþlemlerini kullanýcý dostu bir arayüzle sunar.
  - HttpClient ve ViewModel/Dtos kullanýmý örneklenmiþtir.
- **Gerekli NuGet Paketleri:**
  - `Microsoft.VisualStudio.Web.CodeGeneration.Design` (v8.0.7)
  - `Newtonsoft.Json` (v13.0.3)

---

## Project03_RapidApi
- **Açýklama:**
  - Konsol uygulamasý olarak geliþtirilmiþtir.
  - RapidAPI üzerinden IMDB Top 100 diziler API'sine baðlanýr ve dizi listesini ekrana yazdýrýr.
  - API anahtarý ile dýþ servis tüketimi ve JSON verisinin modele dönüþtürülmesi örneklenmiþtir.
- **Gerekli NuGet Paketleri:**
  - `Newtonsoft.Json` (v13.0.3)

---

## Project04_ConsoleAIChat
- **Açýklama:**
  - Konsol uygulamasý olarak geliþtirilmiþtir.
  - Google Gemini API ile ücretsiz olarak sohbet (chatbot) deneyimi sunar.
  - API anahtarý güvenli þekilde appsettings.json dosyasýndan okunur.
  - Kullanýcý ve yapay zeka mesajlarý renkli ve geçmiþli olarak gösterilir.
  - Microsoft.Extensions.Configuration.Json paketi ile yapýlandýrma yönetimi örneklenmiþtir.
- **Gerekli NuGet Paketleri:**
  - `Microsoft.Extensions.Configuration.Json` (v8.0.1)

---

## Project05_ConsoleSpeechToText
- **Açýklama:**
  - Konsol uygulamasý olarak geliþtirilmiþtir.
  - AssemblyAI API ile ses dosyasýný metne çevirir (Speech-to-Text).
  - API anahtarý appsettings.json dosyasýndan okunur.
  - Kullanýcýdan ses dosyasý adý alýnabilir, renkli ve adým adým bilgilendirme yapýlýr.
  - Baþarýlý ve baþarýsýz durumlar renkli olarak gösterilir, bekleme animasyonu eklenmiþtir.
- **Gerekli NuGet Paketleri:**
  - `Microsoft.Extensions.Configuration.Json` (v8.0.1)
- **Örnek Ses Dosyalarý:**
  - `audio1.mp3` ve `audio2.mp3` dosyalarý test için proje ile birlikte gelir

---

## Project06_ConsoleImageGeneration
- **Açýklama:**
  - Konsol uygulamasý olarak geliþtirilmiþtir.
  - Kullanýcýdan prompt alýp iki farklý model ile görsel üretir:
    - **Stable Diffusion XL (Hugging Face)**: Sadece Ýngilizce, görseli bilgisayara kaydeder.
    - **FLUX Text To Image (modelslab.com)**: Türkçe/Ýngilizce, görselin URL'sini döndürür.
  - API anahtarlarýný `appsettings.json` dosyasýna eklemelisiniz. Örnek için `appsettings.example.json` dosyasýna bakabilirsiniz.
- **Gerekli NuGet Paketleri:**
  - `Microsoft.Extensions.Configuration` (v9.0.7)
  - `Microsoft.Extensions.Configuration.Json` (v8.0.1)

---

## Project07_TesseractOcrConsole
- **Açýklama:**
  - Konsol uygulamasý olarak geliþtirilmiþtir.
  - Tesseract OCR ile görüntü dosyalarýndaki metinleri okur ve analiz eder.
  - Türkçe ve Ýngilizce dil desteði, akýllý dosya seçimi ve detaylý analiz raporu sunar.
  - OCR sonuçlarýný dosyaya kaydetme özelliði ve konsol içi kurulum rehberi bulunur.
- **Gerekli NuGet Paketleri:**
  - `Tesseract` (v5.2.0)

---

## Project08_GoogleVisionOcrConsole
- **Açýklama:**
  - Konsol uygulamasý olarak geliþtirilmiþtir.
  - Google Cloud Vision API ile resimlerden metin çýkarma (OCR) iþlemi yapar.
  - Images klasöründeki resimleri otomatik listeler ve seçim yapmayý kolaylaþtýrýr.
  - OCR sonuçlarýný dosyaya kaydetme özelliði bulunur.
  - Google Cloud Service Account JSON dosyasý gereklidir.
- **Gerekli NuGet Paketleri:**
  - `Google.Cloud.Vision.V1` (v3.14.0)

---

## Project09_TranslatorAIConsole
- **Açýklama:**
  - Konsol uygulamasý olarak geliþtirilmiþtir.
  - Google AI Studio (Gemini) API ile çok dilli çeviri iþlemleri yapar.
  - 7 farklý dil desteði (Türkçe, Ýngilizce, Almanca, Fransýzca, Ýspanyolca, Ýtalyanca, Portekizce) bulunur.
  - Hýzlý çeviri özelliði ile sadece Türkçe'den Ýngilizce'ye çeviri yapar.
  - Özel dil seçimi ile kullanýcý kaynak ve hedef dili manuel olarak belirleyebilir.
  - Sürekli çeviri döngüsü ile 'q' yazýlana kadar çeviri yapmaya devam eder.
  - API anahtarý güvenli þekilde appsettings.json dosyasýndan okunur.
- **Gerekli NuGet Paketleri:**
  - `Microsoft.Extensions.Configuration.Json` (v8.0.1)
  - `System.Text.Json` (v8.0.6)

---

## Project10_TextToSpeechConsole
- **Açýklama:**
  - Konsol uygulamasý olarak geliþtirilmiþtir.
  - System.Speech.Synthesis kullanarak metni sese çevirir (Text-to-Speech).
  - Geliþmiþ kullanýcý arayüzü ile ses seçimi, hýz ve volüm ayarlarý bulunur.
  - Varsayýlan ayarlarla hýzlý baþlangýç, isteðe baðlý detaylý ayar menüsü sunar.
  - Mevcut sistemdeki tüm TTS seslerini listeler ve seçim yapma imkaný verir.
  - Türkçe ses desteði (Windows'ta Türkçe TTS sesi varsa otomatik algýlar).
  - Sürekli metin girme döngüsü ile çoklu kullaným olanaðý saðlar.
- **Gerekli NuGet Paketleri:**
  - `System.Speech` (Windows built-in)

---

## Project11_ConsoleTextToSpeechWithGoogleCloudAPI
- **Açýklama:**
  - Konsol uygulamasý olarak geliþtirilmiþtir.
  - Google Cloud Text-to-Speech API ile profesyonel metinden sese çevirme iþlemleri yapar.
  - 7 farklý dil desteði (Türkçe, Ýngilizce (ABD/Ýngiltere), Almanca, Fransýzca, Ýspanyolca, Ýtalyanca) bulunur.
  - **Özellikler:**
    - **Hýzlý Seslendirme**: Varsayýlan Türkçe kadýn sesi ile basit TTS
    - **Geliþmiþ Seslendirme**: Dil, ses, hýz, ton ve format seçimi ile özelleþtirilebilir TTS
    - **Batch Ýþleme**: Çoklu metni ayný anda iþleyerek ayrý dosyalar oluþturma
    - **Mevcut Sesler**: Google Cloud'daki tüm mevcut sesleri listeleme
    - **Dosyadan Okuma**: .txt dosyalarýndan metin okuyarak seslendirme
  - Konuþma hýzý (0.5x - 1.5x), ses tonu (-5.0 ile +5.0 arasý) ve format (MP3/WAV) seçenekleri mevcuttur.
  - Google Cloud Service Account JSON dosyasý gereklidir (`google-cloud-credentials-example.json` dosyasýna bakýn).
- **Gerekli NuGet Paketleri:**
  - `Google.Cloud.TextToSpeech.V1` (v3.12.0)

---

Her proje kendi klasöründe, ilgili kod ve yapýlandýrma dosyalarýný içermektedir. Yeni projeler eklendikçe bu README dosyasý güncellenecektir.