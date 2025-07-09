# NetCoreAI Proje Serisi

Bu depo, .NET 8 ve C# 12 ile geliþtirilmiþ, temelden ileri seviyeye çeþitli API ve yapay zeka entegrasyonlarýný içeren 5 farklý projeyi barýndýrmaktadýr. Her bir proje, farklý bir teknolojik konsepti ve uygulama örneðini göstermektedir.

---

## Project01_ApiDemo
- **Açýklama:**
  - Basit bir ASP.NET Core Web API projesidir.
  - Müþteri (Customer) CRUD iþlemleri (ekle, listele, güncelle, sil) için Entity Framework Core ile SQLite veritabaný kullanýr.
  - Temel RESTful servis mantýðý ve controller yapýsý örneklenmiþtir.

---

## Project02_ApiConsumeUI
- **Açýklama:**
  - ASP.NET Core Razor Pages tabanlý bir web arayüzüdür.
  - Project01'deki API'yi tüketerek müþteri ekleme, listeleme, güncelleme ve silme iþlemlerini kullanýcý dostu bir arayüzle sunar.
  - HttpClient ve ViewModel/Dtos kullanýmý örneklenmiþtir.

---

## Project03_RapidApi
- **Açýklama:**
  - Konsol uygulamasý olarak geliþtirilmiþtir.
  - RapidAPI üzerinden IMDB Top 100 diziler API'sine baðlanýr ve dizi listesini ekrana yazdýrýr.
  - API anahtarý ile dýþ servis tüketimi ve JSON verisinin modele dönüþtürülmesi örneklenmiþtir.

---

## Project04_ConsoleAIChat
- **Açýklama:**
  - Konsol uygulamasý olarak geliþtirilmiþtir.
  - Google Gemini API ile ücretsiz olarak sohbet (chatbot) deneyimi sunar.
  - API anahtarý güvenli þekilde appsettings.json dosyasýndan okunur.
  - Kullanýcý ve yapay zeka mesajlarý renkli ve geçmiþli olarak gösterilir.
  - Microsoft.Extensions.Configuration.Json paketi ile yapýlandýrma yönetimi örneklenmiþtir.

---

## Project05_ConsoleSpeechToText
- **Açýklama:**
  - Konsol uygulamasý olarak geliþtirilmiþtir.
  - AssemblyAI API ile ses dosyasýný metne çevirir (Speech-to-Text).
  - API anahtarý appsettings.json dosyasýndan okunur.
  - Kullanýcýdan ses dosyasý adý alýnabilir, renkli ve adým adým bilgilendirme yapýlýr.
  - Baþarýlý ve baþarýsýz durumlar renkli olarak gösterilir, bekleme animasyonu eklenmiþtir.

---

Her proje kendi klasöründe, ilgili kod ve yapýlandýrma dosyalarýný içermektedir. Yeni projeler eklendikçe bu README dosyasý güncellenecektir.
