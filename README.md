# .NET Core AI Projeleri

Bu repo, �e�itli .NET Core teknolojileri ile AI entegrasyonu �rneklerini i�ermektedir. Her proje, farkl� bir AI hizmeti veya teknolojisi ile entegrasyon �rne�i sunmaktad�r.

## Projeler

## Project01_ApiDemo
- **A��klama:** 
  - Basit bir ASP.NET Core Web API projesidir.
  - M��teri (Customer) CRUD i�lemleri (ekle, listele, g�ncelle, sil) i�in Entity Framework Core ile SQLite veritaban� kullan�r.
  - Temel RESTful servis mant��� ve controller yap�s� �rneklenmi�tir.
- **Gerekli NuGet Paketleri:**
  - `Microsoft.EntityFrameworkCore` (v8.0.17)
  - `Microsoft.EntityFrameworkCore.Design` (v8.0.17)
  - `Microsoft.EntityFrameworkCore.SqlServer` (v8.0.17)
  - `Microsoft.EntityFrameworkCore.Tools` (v8.0.17)
  - `Swashbuckle.AspNetCore` (v8.1.4)

---

## Project02_ApiConsumeUI
- **A��klama:**
  - ASP.NET Core Razor Pages tabanl� bir web aray�z�d�r.
  - Project01'deki API'yi t�keterek m��teri ekleme, listeleme, g�ncelleme ve silme i�lemlerini kullan�c� dostu bir aray�zle sunar.
  - HttpClient ve ViewModel/Dtos kullan�m� �rneklenmi�tir.
- **Gerekli NuGet Paketleri:**
  - `Microsoft.VisualStudio.Web.CodeGeneration.Design` (v8.0.7)
  - `Newtonsoft.Json` (v13.0.3)

---

## Project03_RapidApi
- **A��klama:**
  - Konsol uygulamas� olarak geli�tirilmi�tir.
  - RapidAPI �zerinden IMDB Top 100 diziler API'sine ba�lan�r ve dizi listesini ekrana yazd�r�r.
  - API anahtar� ile d�� servis t�ketimi ve JSON verisinin modele d�n��t�r�lmesi �rneklenmi�tir.
- **Gerekli NuGet Paketleri:**
  - `Newtonsoft.Json` (v13.0.3)

---

## Project04_ConsoleAIChat
- **A��klama:**
  - Konsol uygulamas� olarak geli�tirilmi�tir.
  - Google Gemini API ile �cretsiz olarak sohbet (chatbot) deneyimi sunar.
  - API anahtar� g�venli �ekilde appsettings.json dosyas�ndan okunur.
  - Kullan�c� ve yapay zeka mesajlar� renkli ve ge�mi�li olarak g�sterilir.
  - Microsoft.Extensions.Configuration.Json paketi ile yap�land�rma y�netimi �rneklenmi�tir.
- **Gerekli NuGet Paketleri:**
  - `Microsoft.Extensions.Configuration.Json` (v8.0.1)

---

## Project05_ConsoleSpeechToText
- **A��klama:**
  - Konsol uygulamas� olarak geli�tirilmi�tir.
  - AssemblyAI API ile ses dosyas�n� metne �evirir (Speech-to-Text).
  - API anahtar� appsettings.json dosyas�ndan okunur.
  - Kullan�c�dan ses dosyas� ad� al�nabilir, renkli ve ad�m ad�m bilgilendirme yap�l�r.
  - Ba�ar�l� ve ba�ar�s�z durumlar renkli olarak g�sterilir, bekleme animasyonu eklenmi�tir.
- **Gerekli NuGet Paketleri:**
  - `Microsoft.Extensions.Configuration.Json` (v8.0.1)
- **�rnek Ses Dosyalar�:**
  - `audio1.mp3` ve `audio2.mp3` dosyalar� test i�in proje ile birlikte gelir

---

## Project06_ConsoleImageGeneration
- **A��klama:**
  - Konsol uygulamas� olarak geli�tirilmi�tir.
  - Kullan�c�dan prompt al�p iki farkl� model ile g�rsel �retir:
    - **Stable Diffusion XL (Hugging Face)**: Sadece �ngilizce, g�rseli bilgisayara kaydeder.
    - **FLUX Text To Image (modelslab.com)**: T�rk�e/�ngilizce, g�rselin URL'sini d�nd�r�r.
  - API anahtarlar�n� `appsettings.json` dosyas�na eklemelisiniz. �rnek i�in `appsettings.example.json` dosyas�na bakabilirsiniz.
- **Gerekli NuGet Paketleri:**
  - `Microsoft.Extensions.Configuration` (v9.0.7)
  - `Microsoft.Extensions.Configuration.Json` (v8.0.1)

---

## Project07_TesseractOcrConsole
- **A��klama:**
  - Konsol uygulamas� olarak geli�tirilmi�tir.
  - Tesseract OCR ile g�r�nt� dosyalar�ndaki metinleri okur ve analiz eder.
  - T�rk�e ve �ngilizce dil deste�i, ak�ll� dosya se�imi ve detayl� analiz raporu sunar.
  - OCR sonu�lar�n� dosyaya kaydetme �zelli�i ve konsol i�i kurulum rehberi bulunur.
- **Gerekli NuGet Paketleri:**
  - `Tesseract` (v5.2.0)

---

## Project08_GoogleVisionOcrConsole
- **A��klama:**
  - Konsol uygulamas� olarak geli�tirilmi�tir.
  - Google Cloud Vision API ile resimlerden metin ��karma (OCR) i�lemi yapar.
  - Images klas�r�ndeki resimleri otomatik listeler ve se�im yapmay� kolayla�t�r�r.
  - OCR sonu�lar�n� dosyaya kaydetme �zelli�i bulunur.
  - Google Cloud Service Account JSON dosyas� gereklidir.
- **Gerekli NuGet Paketleri:**
  - `Google.Cloud.Vision.V1` (v3.14.0)

---

## Project09_TranslatorAIConsole
- **A��klama:**
  - Konsol uygulamas� olarak geli�tirilmi�tir.
  - Google AI Studio (Gemini) API ile �ok dilli �eviri i�lemleri yapar.
  - 7 farkl� dil deste�i (T�rk�e, �ngilizce, Almanca, Frans�zca, �spanyolca, �talyanca, Portekizce) bulunur.
  - H�zl� �eviri �zelli�i ile sadece T�rk�e'den �ngilizce'ye �eviri yapar.
  - �zel dil se�imi ile kullan�c� kaynak ve hedef dili manuel olarak belirleyebilir.
  - S�rekli �eviri d�ng�s� ile 'q' yaz�lana kadar �eviri yapmaya devam eder.
  - API anahtar� g�venli �ekilde appsettings.json dosyas�ndan okunur.
- **Gerekli NuGet Paketleri:**
  - `Microsoft.Extensions.Configuration.Json` (v8.0.1)
  - `System.Text.Json` (v8.0.6)

---

## Project10_TextToSpeechConsole
- **A��klama:**
  - Konsol uygulamas� olarak geli�tirilmi�tir.
  - System.Speech.Synthesis kullanarak metni sese �evirir (Text-to-Speech).
  - Geli�mi� kullan�c� aray�z� ile ses se�imi, h�z ve vol�m ayarlar� bulunur.
  - Varsay�lan ayarlarla h�zl� ba�lang��, iste�e ba�l� detayl� ayar men�s� sunar.
  - Mevcut sistemdeki t�m TTS seslerini listeler ve se�im yapma imkan� verir.
  - T�rk�e ses deste�i (Windows'ta T�rk�e TTS sesi varsa otomatik alg�lar).
  - S�rekli metin girme d�ng�s� ile �oklu kullan�m olana�� sa�lar.
- **Gerekli NuGet Paketleri:**
  - `System.Speech` (Windows built-in)

---

## Project11_ConsoleTextToSpeechWithGoogleCloudAPI
- **A��klama:**
  - Konsol uygulamas� olarak geli�tirilmi�tir.
  - Google Cloud Text-to-Speech API ile profesyonel metinden sese �evirme i�lemleri yapar.
  - 7 farkl� dil deste�i (T�rk�e, �ngilizce (ABD/�ngiltere), Almanca, Frans�zca, �spanyolca, �talyanca) bulunur.
  - **�zellikler:**
    - **H�zl� Seslendirme**: Varsay�lan T�rk�e kad�n sesi ile basit TTS
    - **Geli�mi� Seslendirme**: Dil, ses, h�z, ton ve format se�imi ile �zelle�tirilebilir TTS
    - **Batch ��leme**: �oklu metni ayn� anda i�leyerek ayr� dosyalar olu�turma
    - **Mevcut Sesler**: Google Cloud'daki t�m mevcut sesleri listeleme
    - **Dosyadan Okuma**: .txt dosyalar�ndan metin okuyarak seslendirme
  - Konu�ma h�z� (0.5x - 1.5x), ses tonu (-5.0 ile +5.0 aras�) ve format (MP3/WAV) se�enekleri mevcuttur.
  - Google Cloud Service Account JSON dosyas� gereklidir (`google-cloud-credentials-example.json` dosyas�na bak�n).
- **Gerekli NuGet Paketleri:**
  - `Google.Cloud.TextToSpeech.V1` (v3.12.0)

---

Her proje kendi klas�r�nde, ilgili kod ve yap�land�rma dosyalar�n� i�ermektedir. Yeni projeler eklendik�e bu README dosyas� g�ncellenecektir.