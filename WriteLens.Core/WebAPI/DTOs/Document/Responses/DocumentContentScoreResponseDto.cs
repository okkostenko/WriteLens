namespace WriteLens.Core.WebAPI.DTOs.Document.Responses;

public class DocumentContentScoreResponseDto
{
    public decimal TotalScore { get; set; }

    public decimal ReadabilityScore { get; set; }

    public decimal AccessibilityScore { get; set; }
    
    public DateTimeOffset LastUpdated { get; set; }
}