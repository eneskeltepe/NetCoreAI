using System.Text;
using System.Text.RegularExpressions;
using Project14_TextSummarizerAIWeb.Models;

namespace Project14_TextSummarizerAIWeb.Services;

public class ExportService
{
    public byte[] ExportToWord(SummaryResult result)
    {
        // Generate proper Word document content in RTF format
        var content = GenerateRtfContent(result);
        return Encoding.UTF8.GetBytes(content);
    }

    public byte[] ExportToDocx(SummaryResult result)
    {
        // Generate HTML content that can be opened by Word as DOCX
        var content = GenerateWordHtmlContent(result);
        return Encoding.UTF8.GetBytes(content);
    }

    public byte[] ExportToText(SummaryResult result)
    {
        var content = GenerateTextContent(result);
        return Encoding.UTF8.GetBytes(content);
    }

    public byte[] ExportToMarkdown(SummaryResult result)
    {
        var content = GenerateMarkdownContent(result);
        return Encoding.UTF8.GetBytes(content);
    }

    public byte[] ExportToHtml(SummaryResult result)
    {
        // Generate PDF-ready HTML content with print functionality
        var html = GeneratePdfReadyHtmlContent(result);
        return Encoding.UTF8.GetBytes(html);
    }

    private string GeneratePdfReadyHtmlContent(SummaryResult result)
    {
        var html = new StringBuilder();
        html.AppendLine("<!DOCTYPE html>");
        html.AppendLine("<html><head>");
        html.AppendLine("<meta charset='utf-8'>");
        html.AppendLine("<meta name='viewport' content='width=device-width, initial-scale=1'>");
        html.AppendLine($"<title>{System.Web.HttpUtility.HtmlEncode(result.Title)}</title>");
        html.AppendLine("<style>");
        html.AppendLine(@"
            /* Screen styles */
            body { 
                font-family: 'Segoe UI', Arial, sans-serif; 
                line-height: 1.6; 
                margin: 0; 
                padding: 20px;
                color: #333; 
                background: #f8f9fa;
            }
            .container {
                max-width: 800px;
                margin: 0 auto;
                background: white;
                padding: 40px;
                border-radius: 8px;
                box-shadow: 0 4px 6px rgba(0,0,0,0.1);
            }
            .header { 
                text-align: center; 
                margin-bottom: 30px; 
                border-bottom: 3px solid #007bff; 
                padding-bottom: 20px; 
            }
            .title { 
                color: #007bff; 
                font-size: 28px; 
                margin-bottom: 10px; 
                font-weight: bold; 
            }
            .meta { 
                color: #666; 
                font-size: 14px; 
            }
            .summary-section { 
                margin: 30px 0; 
                padding: 20px; 
                border-radius: 8px;
                border-left: 5px solid;
                background-color: #f8f9fa; 
            }
            .summary-section.short {
                border-left-color: #17a2b8;
                background-color: #e1f5fe;
            }
            .summary-section.medium {
                border-left-color: #ffc107;
                background-color: #fff8e1;
            }
            .summary-section.detailed {
                border-left-color: #28a745;
                background-color: #e8f5e8;
            }
            .summary-title { 
                font-size: 18px; 
                margin-bottom: 15px; 
                font-weight: bold; 
            }
            .summary-title.short { color: #17a2b8; }
            .summary-title.medium { color: #e67e22; }
            .summary-title.detailed { color: #28a745; }
            .summary-content { 
                font-size: 16px; 
                line-height: 1.8; 
                text-align: justify;
            }
            .stats { 
                margin: 30px 0; 
                padding: 20px; 
                background-color: #f0f0f0; 
                border-radius: 8px; 
            }
            .stats-title {
                text-align: center; 
                color: #007bff; 
                margin-bottom: 20px;
                font-weight: bold;
                font-size: 18px;
            }
            .stats-row { 
                display: flex; 
                justify-content: space-around; 
            }
            .stat-item { 
                text-align: center; 
                flex: 1; 
            }
            .stat-number { 
                font-size: 24px; 
                font-weight: bold; 
                color: #007bff; 
                display: block; 
            }
            .stat-label { 
                font-size: 12px; 
                color: #666; 
                margin-top: 5px; 
            }
            
            .print-button {
                position: fixed;
                top: 20px;
                right: 20px;
                background: #007bff;
                color: white;
                border: none;
                padding: 12px 20px;
                border-radius: 50px;
                cursor: pointer;
                font-size: 14px;
                font-weight: bold;
                box-shadow: 0 4px 12px rgba(0,123,255,0.3);
                transition: all 0.3s ease;
                z-index: 1000;
            }
            .print-button:hover {
                background: #0056b3;
                transform: translateY(-2px);
                box-shadow: 0 6px 20px rgba(0,123,255,0.4);
            }

            /* Print styles */
            @media print {
                body { 
                    background: white !important;
                    padding: 0 !important;
                }
                .container {
                    max-width: none !important;
                    box-shadow: none !important;
                    border-radius: 0 !important;
                    padding: 20px !important;
                }
                .print-button {
                    display: none !important;
                }
                .summary-section {
                    page-break-inside: avoid;
                    border-left: 3px solid;
                }
                .stats {
                    page-break-inside: avoid;
                }
                @page {
                    margin: 2cm;
                    size: A4;
                }
            }
        ");
        html.AppendLine("</style></head><body>");
        
        // Print Button (soru işaretleri kaldırıldı)
        html.AppendLine("<button class='print-button' onclick='window.print()'>");
        html.AppendLine("PDF Olarak Kaydet");
        html.AppendLine("</button>");
        
        html.AppendLine("<div class='container'>");
        
        // Header
        html.AppendLine("<div class='header'>");
        html.AppendLine($"<h1 class='title'>{System.Web.HttpUtility.HtmlEncode(result.Title)}</h1>");
        html.AppendLine($"<div class='meta'>Oluşturulma: {result.CreatedAt:dd.MM.yyyy HH:mm} | Dil: {(result.Language == "tr" ? "Türkçe" : "English")} | ID: {result.Id}</div>");
        html.AppendLine("</div>");

        // Summaries with different colors
        if (!string.IsNullOrEmpty(result.ShortSummary))
        {
            html.AppendLine("<div class='summary-section short'>");
            html.AppendLine("<h2 class='summary-title short'>Kısa Özet</h2>");
            html.AppendLine($"<div class='summary-content'>{System.Web.HttpUtility.HtmlEncode(result.ShortSummary)}</div>");
            html.AppendLine("</div>");
        }

        if (!string.IsNullOrEmpty(result.MediumSummary))
        {
            html.AppendLine("<div class='summary-section medium'>");
            html.AppendLine("<h2 class='summary-title medium'>Orta Özet</h2>");
            html.AppendLine($"<div class='summary-content'>{System.Web.HttpUtility.HtmlEncode(result.MediumSummary)}</div>");
            html.AppendLine("</div>");
        }

        if (!string.IsNullOrEmpty(result.DetailedSummary))
        {
            html.AppendLine("<div class='summary-section detailed'>");
            html.AppendLine("<h2 class='summary-title detailed'>Detaylı Özet</h2>");
            html.AppendLine($"<div class='summary-content'>{System.Web.HttpUtility.HtmlEncode(result.DetailedSummary)}</div>");
            html.AppendLine("</div>");
        }

        // Statistics
        html.AppendLine("<div class='stats'>");
        html.AppendLine("<div class='stats-title'>İstatistikler</div>");
        html.AppendLine("<div class='stats-row'>");
        html.AppendLine($"<div class='stat-item'><span class='stat-number'>{result.OriginalCharCount:N0}</span><div class='stat-label'>Karakter Sayısı</div></div>");
        html.AppendLine($"<div class='stat-item'><span class='stat-number'>{result.OriginalWordCount:N0}</span><div class='stat-label'>Kelime Sayısı</div></div>");
        html.AppendLine($"<div class='stat-item'><span class='stat-number'>{result.CreatedAt:HH:mm}</span><div class='stat-label'>Oluşturulma Saati</div></div>");
        html.AppendLine("</div>");
        html.AppendLine("</div>");

        // Footer
        html.AppendLine("<div style='text-align: center; margin-top: 30px; color: #666; font-size: 12px;'>");
        html.AppendLine("AI Text Summarizer ile oluşturulmuştur");
        html.AppendLine("</div>");

        html.AppendLine("</div>");
        html.AppendLine("</body></html>");
        return html.ToString();
    }

    private string GenerateWordHtmlContent(SummaryResult result)
    {
        var html = new StringBuilder();
        html.AppendLine("<!DOCTYPE html>");
        html.AppendLine("<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:word' xmlns='http://www.w3.org/TR/REC-html40'>");
        html.AppendLine("<head>");
        html.AppendLine("<meta charset='utf-8'>");
        html.AppendLine("<meta name='ProgId' content='Word.Document'>");
        html.AppendLine("<meta name='Generator' content='Microsoft Word'>");
        html.AppendLine("<meta name='Originator' content='Microsoft Word'>");
        html.AppendLine($"<title>{System.Web.HttpUtility.HtmlEncode(result.Title)}</title>");
        html.AppendLine("<style>");
        html.AppendLine(@"
            @page { margin: 2.5cm; }
            body { font-family: 'Calibri', 'Arial', sans-serif; font-size: 11pt; line-height: 1.15; margin: 0; }
            .title { font-size: 18pt; font-weight: bold; color: #1f4e79; text-align: center; margin-bottom: 12pt; }
            .meta { font-size: 9pt; color: #666666; text-align: center; margin-bottom: 18pt; }
            .summary-section { margin: 18pt 0; }
            .summary-title { font-size: 14pt; font-weight: bold; margin-bottom: 6pt; }
            .summary-title.short { color: #17a2b8; }
            .summary-title.medium { color: #e67e22; }
            .summary-title.detailed { color: #28a745; }
            .summary-content { font-size: 11pt; text-align: justify; margin-bottom: 12pt; }
            .stats-table { width: 100%; border-collapse: collapse; margin: 18pt 0; }
            .stats-table td { padding: 6pt; text-align: center; border: 1pt solid #cccccc; }
            .stats-header { background-color: #f2f2f2; font-weight: bold; }
        ");
        html.AppendLine("</style></head><body>");
        
        // Header
        html.AppendLine($"<div class='title'>{System.Web.HttpUtility.HtmlEncode(result.Title)}</div>");
        html.AppendLine($"<div class='meta'>Oluşturulma: {result.CreatedAt:dd.MM.yyyy HH:mm} | Dil: {(result.Language == "tr" ? "Türkçe" : "English")}</div>");

        // Summaries with colored titles
        if (!string.IsNullOrEmpty(result.ShortSummary))
        {
            html.AppendLine("<div class='summary-section'>");
            html.AppendLine("<div class='summary-title short'>Kısa Özet</div>");
            html.AppendLine($"<div class='summary-content'>{System.Web.HttpUtility.HtmlEncode(result.ShortSummary)}</div>");
            html.AppendLine("</div>");
        }

        if (!string.IsNullOrEmpty(result.MediumSummary))
        {
            html.AppendLine("<div class='summary-section'>");
            html.AppendLine("<div class='summary-title medium'>Orta Özet</div>");
            html.AppendLine($"<div class='summary-content'>{System.Web.HttpUtility.HtmlEncode(result.MediumSummary)}</div>");
            html.AppendLine("</div>");
        }

        if (!string.IsNullOrEmpty(result.DetailedSummary))
        {
            html.AppendLine("<div class='summary-section'>");
            html.AppendLine("<div class='summary-title detailed'>Detaylı Özet</div>");
            html.AppendLine($"<div class='summary-content'>{System.Web.HttpUtility.HtmlEncode(result.DetailedSummary)}</div>");
            html.AppendLine("</div>");
        }

        // Statistics Table
        html.AppendLine("<table class='stats-table'>");
        html.AppendLine("<tr class='stats-header'><td>İstatistik</td><td>Değer</td></tr>");
        html.AppendLine($"<tr><td>Karakter Sayısı</td><td>{result.OriginalCharCount:N0}</td></tr>");
        html.AppendLine($"<tr><td>Kelime Sayısı</td><td>{result.OriginalWordCount:N0}</td></tr>");
        html.AppendLine($"<tr><td>Oluşturulma Saati</td><td>{result.CreatedAt:HH:mm}</td></tr>");
        html.AppendLine("</table>");

        html.AppendLine("<div style='text-align: center; font-size: 9pt; color: #666666; margin-top: 18pt;'>");
        html.AppendLine("AI Text Summarizer ile oluşturulmuştur");
        html.AppendLine("</div>");

        html.AppendLine("</body></html>");
        return html.ToString();
    }

    private string GenerateRtfContent(SummaryResult result)
    {
        var rtf = new StringBuilder();
        
        // RTF header
        rtf.AppendLine(@"{\rtf1\ansi\deff0 {\fonttbl {\f0 Times New Roman;} {\f1 Arial;}}");
        rtf.AppendLine(@"{\colortbl;\red0\green123\blue255;\red51\green51\blue51;\red23\green162\blue184;\red230\green126\blue34;\red40\green167\blue69;}");
        
        // Title
        rtf.AppendLine(@"\f1\fs28\cf1\b " + EscapeRtf(result.Title) + @"\b0\par");
        rtf.AppendLine(@"\fs18\cf2 Olu\u351?turulma: " + result.CreatedAt.ToString("dd.MM.yyyy HH:mm") + 
                      @" | Dil: " + (result.Language == "tr" ? "Türkçe" : "English") + @"\par\par");

        // Summaries with different colors
        if (!string.IsNullOrEmpty(result.ShortSummary))
        {
            rtf.AppendLine(@"\fs20\cf3\b K\u305?sa Özet\b0\par");
            rtf.AppendLine(@"\fs16\cf2 " + EscapeRtf(result.ShortSummary) + @"\par\par");
        }

        if (!string.IsNullOrEmpty(result.MediumSummary))
        {
            rtf.AppendLine(@"\fs20\cf4\b Orta Özet\b0\par");
            rtf.AppendLine(@"\fs16\cf2 " + EscapeRtf(result.MediumSummary) + @"\par\par");
        }

        if (!string.IsNullOrEmpty(result.DetailedSummary))
        {
            rtf.AppendLine(@"\fs20\cf5\b Detayl\u305? Özet\b0\par");
            rtf.AppendLine(@"\fs16\cf2 " + EscapeRtf(result.DetailedSummary) + @"\par\par");
        }

        // Statistics
        rtf.AppendLine(@"\fs20\cf1\b \u304?statistikler\b0\par");
        rtf.AppendLine(@"\fs16\cf2 Karakter Say\u305?s\u305?: " + result.OriginalCharCount.ToString("N0") + @"\par");
        rtf.AppendLine(@"\fs16\cf2 Kelime Say\u305?s\u305?: " + result.OriginalWordCount.ToString("N0") + @"\par\par");
        
        // Footer
        rtf.AppendLine(@"\fs14\cf2\i AI Text Summarizer ile olu\u351?turulmu\u351?tur\i0\par");
        rtf.AppendLine("}");

        return rtf.ToString();
    }

    private string EscapeRtf(string text)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;
        
        // RTF special characters escape
        text = text.Replace(@"\", @"\\");
        text = text.Replace("{", @"\{");
        text = text.Replace("}", @"\}");
        
        // Turkish characters to Unicode
        text = text.Replace("ç", @"\u231?");
        text = text.Replace("ğ", @"\u287?");
        text = text.Replace("ı", @"\u305?");
        text = text.Replace("ö", @"\u246?");
        text = text.Replace("ş", @"\u351?");
        text = text.Replace("ü", @"\u252?");
        text = text.Replace("Ç", @"\u199?");
        text = text.Replace("Ğ", @"\u286?");
        text = text.Replace("I", @"\u304?");
        text = text.Replace("Ö", @"\u214?");
        text = text.Replace("Ş", @"\u350?");
        text = text.Replace("Ü", @"\u220?");

        return text;
    }

    private string GenerateTextContent(SummaryResult result)
    {
        var content = new StringBuilder();
        content.AppendLine($"{result.Title}");
        content.AppendLine(new string('=', result.Title.Length));
        content.AppendLine();
        content.AppendLine($"Oluşturulma: {result.CreatedAt:dd.MM.yyyy HH:mm}");
        content.AppendLine($"Dil: {(result.Language == "tr" ? "Türkçe" : "English")}");
        content.AppendLine($"Karakter Sayısı: {result.OriginalCharCount:N0}");
        content.AppendLine($"Kelime Sayısı: {result.OriginalWordCount:N0}");
        content.AppendLine();

        if (!string.IsNullOrEmpty(result.ShortSummary))
        {
            content.AppendLine("KISA ÖZET");
            content.AppendLine("---------");
            content.AppendLine(result.ShortSummary);
            content.AppendLine();
        }

        if (!string.IsNullOrEmpty(result.MediumSummary))
        {
            content.AppendLine("ORTA ÖZET");
            content.AppendLine("---------");
            content.AppendLine(result.MediumSummary);
            content.AppendLine();
        }

        if (!string.IsNullOrEmpty(result.DetailedSummary))
        {
            content.AppendLine("DETAYLI ÖZET");
            content.AppendLine("------------");
            content.AppendLine(result.DetailedSummary);
            content.AppendLine();
        }

        content.AppendLine("--");
        content.AppendLine("AI Text Summarizer ile oluşturulmuştur");

        return content.ToString();
    }

    private string GenerateMarkdownContent(SummaryResult result)
    {
        var content = new StringBuilder();
        content.AppendLine($"# {result.Title}\n");
        content.AppendLine($"**Oluşturulma:** {result.CreatedAt:dd.MM.yyyy HH:mm}  ");
        content.AppendLine($"**Dil:** {(result.Language == "tr" ? "Türkçe" : "English")}  ");
        content.AppendLine($"**Karakter Sayısı:** {result.OriginalCharCount:N0}  ");
        content.AppendLine($"**Kelime Sayısı:** {result.OriginalWordCount:N0}  \n");

        if (!string.IsNullOrEmpty(result.ShortSummary))
        {
            content.AppendLine("## Kısa Özet\n");
            content.AppendLine($"> {result.ShortSummary}\n");
        }

        if (!string.IsNullOrEmpty(result.MediumSummary))
        {
            content.AppendLine("## Orta Özet\n");
            content.AppendLine($"> {result.MediumSummary}\n");
        }

        if (!string.IsNullOrEmpty(result.DetailedSummary))
        {
            content.AppendLine("## Detaylı Özet\n");
            content.AppendLine($"> {result.DetailedSummary}\n");
        }

        content.AppendLine("---");
        content.AppendLine("*AI Text Summarizer ile oluşturulmuştur*");

        return content.ToString();
    }

    public string GetSafeFileName(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return $"ozet_{DateTime.Now:yyyyMMdd_HHmmss}";
        }

        // 1. Türkçe karakterleri İngilizce karşılıklarına çevir
        var turkishChars = "çğıöşüÇĞIİÖŞÜ";
        var englishChars = "cgiosuCGIIOSU";
        
        var normalizedTitle = title;
        for (int i = 0; i < turkishChars.Length; i++)
        {
            normalizedTitle = normalizedTitle.Replace(turkishChars[i], englishChars[i]);
        }

        // 2. Sadece alfanumerik karakterler, tire ve alt çizgi bırak
        normalizedTitle = Regex.Replace(normalizedTitle, @"[^a-zA-Z0-9\-_\s]", "");

        // 3. Boşlukları alt çizgi ile değiştir
        normalizedTitle = Regex.Replace(normalizedTitle, @"\s+", "_");

        // 4. Birden fazla alt çizgiyi tek alt çizgi yap
        normalizedTitle = Regex.Replace(normalizedTitle, @"_+", "_");

        // 5. Başındaki ve sonundaki alt çizgileri temizle
        normalizedTitle = normalizedTitle.Trim('_');

        // 6. Boşsa veya çok kısaysa varsayılan isim kullan
        if (string.IsNullOrWhiteSpace(normalizedTitle) || normalizedTitle.Length < 3)
        {
            normalizedTitle = "ai_text_summary";
        }

        // 7. Uzunluk sınırla (40 karakter)
        if (normalizedTitle.Length > 40)
        {
            normalizedTitle = normalizedTitle.Substring(0, 40).TrimEnd('_');
        }

        // 8. Zaman damgası ekle
        return $"{normalizedTitle}_{DateTime.Now:yyyyMMdd_HHmmss}";
    }
}