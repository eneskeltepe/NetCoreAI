using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project14_TextSummarizerAIWeb.Models;
using Project14_TextSummarizerAIWeb.Services;
using System.Text.Json;
using System.Text;

namespace Project14_TextSummarizerAIWeb.Pages;

public class IndexModel : PageModel
{
    private readonly GeminiSummaryService _summaryService;
    private readonly ExportService _exportService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(GeminiSummaryService summaryService, ExportService exportService, ILogger<IndexModel> logger)
    {
        _summaryService = summaryService;
        _exportService = exportService;
        _logger = logger;
    }

    [BindProperty]
    public SummaryRequest SummaryRequest { get; set; } = new();
    
    public SummaryResult? SummaryResult { get; set; }

    public void OnGet()
    {
        // Sayfa ilk yüklendiğinde
    }

    public async Task<IActionResult> OnPostSummarizeAsync()
    {
        try
        {
            // File upload validation
            if (SummaryRequest.UploadedFile != null)
            {
                // File size validation (10MB)
                if (SummaryRequest.UploadedFile.Length > 10 * 1024 * 1024)
                {
                    ModelState.AddModelError("SummaryRequest.UploadedFile", "Dosya boyutu 10MB'dan büyük olamaz.");
                    return Page();
                }

                // File type validation
                var allowedExtensions = new[] { ".txt", ".pdf", ".docx" };
                var fileExtension = Path.GetExtension(SummaryRequest.UploadedFile.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("SummaryRequest.UploadedFile", "Sadece TXT, PDF ve DOCX dosyaları desteklenir.");
                    return Page();
                }

                // Read file content
                using var reader = new StreamReader(SummaryRequest.UploadedFile.OpenReadStream());
                var fileContent = await reader.ReadToEndAsync();
                
                if (string.IsNullOrWhiteSpace(fileContent))
                {
                    ModelState.AddModelError("SummaryRequest.UploadedFile", "Dosya içeriği boş.");
                    return Page();
                }

                SummaryRequest.Text = fileContent;
            }

            // Text validation
            if (string.IsNullOrWhiteSpace(SummaryRequest.Text))
            {
                ModelState.AddModelError("SummaryRequest.Text", "Lütfen metin girin veya dosya yükleyin.");
                return Page();
            }

            if (SummaryRequest.Text.Length < 50)
            {
                ModelState.AddModelError("SummaryRequest.Text", "Metin en az 50 karakter olmalıdır.");
                return Page();
            }

            if (SummaryRequest.Text.Length > 100000)
            {
                ModelState.AddModelError("SummaryRequest.Text", "Metin en fazla 100,000 karakter olabilir.");
                return Page();
            }

            // Summary type validation
            if (!SummaryRequest.HasAnySummaryTypeSelected)
            {
                ModelState.AddModelError("", "En az bir özet türü seçmelisiniz.");
                return Page();
            }

            // Language validation
            var allowedLanguages = new[] { "tr", "en" };
            if (!allowedLanguages.Contains(SummaryRequest.Language))
            {
                ModelState.AddModelError("SummaryRequest.Language", "Geçersiz dil seçimi.");
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _logger.LogInformation("Starting summary generation for text with {CharCount} characters", SummaryRequest.Text.Length);

            SummaryResult = await _summaryService.SummarizeTextAsync(SummaryRequest);

            if (SummaryResult.IsSuccess)
            {
                _logger.LogInformation("Summary generation completed successfully. Title: {Title}", SummaryResult.Title);
            }
            else
            {
                _logger.LogWarning("Summary generation failed: {ErrorMessage}", SummaryResult.ErrorMessage);
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during summary generation");
            ModelState.AddModelError("", "Özet oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.");
            return Page();
        }
    }

    public IActionResult OnPostExport(string format, string resultJson)
    {
        try
        {
            _logger.LogInformation("Export request: Format={Format}, HasData={HasData}", format, !string.IsNullOrEmpty(resultJson));

            if (string.IsNullOrWhiteSpace(format))
            {
                _logger.LogWarning("Export format is empty");
                return BadRequest("Export format is required");
            }

            if (string.IsNullOrWhiteSpace(resultJson))
            {
                _logger.LogWarning("Export data is empty");
                return BadRequest("Export data not found");
            }

            var summaryResult = JsonSerializer.Deserialize<SummaryResult>(resultJson);
            if (summaryResult == null)
            {
                _logger.LogWarning("Failed to deserialize export data");
                return BadRequest("Invalid export data");
            }

            _logger.LogInformation("Exporting summary: {Title} in format: {Format}", summaryResult.Title, format);

            // Safe filename generation
            var safeFileName = _exportService.GetSafeFileName(summaryResult.Title);
            
            var (content, fileName, contentType) = format.ToLower() switch
            {
                "html" => (_exportService.ExportToHtml(summaryResult), $"{safeFileName}.html", "text/html"),
                "txt" => (_exportService.ExportToText(summaryResult), $"{safeFileName}.txt", "text/plain"),
                "md" => (_exportService.ExportToMarkdown(summaryResult), $"{safeFileName}.md", "text/markdown"),
                "docx" => (_exportService.ExportToDocx(summaryResult), $"{safeFileName}.doc", "application/msword"),
                "word" => (_exportService.ExportToWord(summaryResult), $"{safeFileName}.rtf", "application/rtf"),
                _ => throw new ArgumentException($"Unsupported format: {format}")
            };

            _logger.LogInformation("Export successful: {FileName}", fileName);

            // URL encode the filename to handle special characters
            var encodedFileName = Uri.EscapeDataString(fileName);
            
            // Set the content disposition header safely
            Response.Headers.ContentDisposition = $"attachment; filename*=UTF-8''{encodedFileName}";
            
            return File(content, contentType, fileName);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON parsing error during export");
            return BadRequest("Invalid JSON data");
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid export format");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during export. Format: {Format}", format);
            return BadRequest("Export failed: " + ex.Message);
        }
    }
}
