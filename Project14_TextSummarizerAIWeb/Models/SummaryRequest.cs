using System.ComponentModel.DataAnnotations;

namespace Project14_TextSummarizerAIWeb.Models;

public class SummaryRequest
{
    [StringLength(100000, MinimumLength = 50, ErrorMessage = "Metin en az 50, en fazla 100,000 karakter olmalıdır.")]
    public string Text { get; set; } = string.Empty;
    
    public bool EnableShort { get; set; } = true;
    public bool EnableMedium { get; set; } = true;
    public bool EnableDetailed { get; set; } = true;
    
    [Required(ErrorMessage = "Dil seçimi zorunludur.")]
    public string Language { get; set; } = "tr";
    
    public IFormFile? UploadedFile { get; set; }

    // Custom validation to ensure at least one summary type is selected
    public bool HasAnySummaryTypeSelected => EnableShort || EnableMedium || EnableDetailed;
}