# NetCoreAI Proje Serisi

Bu depo, .NET 8 ve C# 12 ile geli�tirilmi�, temelden ileri seviyeye �e�itli API ve yapay zeka entegrasyonlar�n� i�eren 8 farkl� projeyi bar�nd�rmaktad�r. Her bir proje, farkl� bir teknolojik konsepti ve uygulama �rne�ini g�stermektedir.

---

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
- **�rnek G�r�nt� Dosyalar�:**
  - `Images/1.png`, `Images/2.jpg`, `Images/3.jpeg` dosyalar� test i�in proje ile birlikte gelir

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
- **�rnek G�r�nt� Dosyalar�:**
  - `Images/` klas�r�ndeki �rnek resim dosyalar� test i�in proje ile birlikte gelir

---

Her proje kendi klas�r�nde, ilgili kod ve yap�land�rma dosyalar�n� i�ermektedir. Yeni projeler eklendik�e bu README dosyas� g�ncellenecektir.