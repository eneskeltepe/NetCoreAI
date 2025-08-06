namespace Project14_TextSummarizerAIWeb.Models;

public class SummaryResult
{
    private static readonly object _lock = new object();

    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    
    // AI Generated Title
    public string Title { get; set; } = string.Empty;
    
    // Summary Content
    public string ShortSummary { get; set; } = string.Empty;
    public string MediumSummary { get; set; } = string.Empty;
    public string DetailedSummary { get; set; } = string.Empty;
    
    // Statistics
    public int OriginalCharCount { get; set; }
    public int OriginalWordCount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string Language { get; set; } = string.Empty;
    
    // For Local Storage - Unique ID System
    public string Id { get; set; }
    public int SequentialId { get; set; }
    public string OriginalTextPreview { get; set; } = string.Empty; // First 200 chars

    public SummaryResult()
    {
        lock (_lock)
        {
            // Timestamp-based unique ID to prevent conflicts
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var guidShort = Guid.NewGuid().ToString("N")[..8];
            
            // Create globally unique ID with timestamp and GUID
            Id = $"summary_{timestamp}_{guidShort}";
            
            // SequentialId will be determined by JavaScript based on localStorage
            SequentialId = 0; // Will be set by JavaScript
        }
    }
}