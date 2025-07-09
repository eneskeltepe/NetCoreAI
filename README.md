# NetCoreAI Proje Serisi

Bu depo, .NET 8 ve C# 12 ile geli�tirilmi�, temelden ileri seviyeye �e�itli API ve yapay zeka entegrasyonlar�n� i�eren 5 farkl� projeyi bar�nd�rmaktad�r. Her bir proje, farkl� bir teknolojik konsepti ve uygulama �rne�ini g�stermektedir.

---

## Project01_ApiDemo
- **A��klama:**
  - Basit bir ASP.NET Core Web API projesidir.
  - M��teri (Customer) CRUD i�lemleri (ekle, listele, g�ncelle, sil) i�in Entity Framework Core ile SQLite veritaban� kullan�r.
  - Temel RESTful servis mant��� ve controller yap�s� �rneklenmi�tir.

---

## Project02_ApiConsumeUI
- **A��klama:**
  - ASP.NET Core Razor Pages tabanl� bir web aray�z�d�r.
  - Project01'deki API'yi t�keterek m��teri ekleme, listeleme, g�ncelleme ve silme i�lemlerini kullan�c� dostu bir aray�zle sunar.
  - HttpClient ve ViewModel/Dtos kullan�m� �rneklenmi�tir.

---

## Project03_RapidApi
- **A��klama:**
  - Konsol uygulamas� olarak geli�tirilmi�tir.
  - RapidAPI �zerinden IMDB Top 100 diziler API'sine ba�lan�r ve dizi listesini ekrana yazd�r�r.
  - API anahtar� ile d�� servis t�ketimi ve JSON verisinin modele d�n��t�r�lmesi �rneklenmi�tir.

---

## Project04_ConsoleAIChat
- **A��klama:**
  - Konsol uygulamas� olarak geli�tirilmi�tir.
  - Google Gemini API ile �cretsiz olarak sohbet (chatbot) deneyimi sunar.
  - API anahtar� g�venli �ekilde appsettings.json dosyas�ndan okunur.
  - Kullan�c� ve yapay zeka mesajlar� renkli ve ge�mi�li olarak g�sterilir.
  - Microsoft.Extensions.Configuration.Json paketi ile yap�land�rma y�netimi �rneklenmi�tir.

---

## Project05_ConsoleSpeechToText
- **A��klama:**
  - Konsol uygulamas� olarak geli�tirilmi�tir.
  - AssemblyAI API ile ses dosyas�n� metne �evirir (Speech-to-Text).
  - API anahtar� appsettings.json dosyas�ndan okunur.
  - Kullan�c�dan ses dosyas� ad� al�nabilir, renkli ve ad�m ad�m bilgilendirme yap�l�r.
  - Ba�ar�l� ve ba�ar�s�z durumlar renkli olarak g�sterilir, bekleme animasyonu eklenmi�tir.

---

Her proje kendi klas�r�nde, ilgili kod ve yap�land�rma dosyalar�n� i�ermektedir. Yeni projeler eklendik�e bu README dosyas� g�ncellenecektir.
