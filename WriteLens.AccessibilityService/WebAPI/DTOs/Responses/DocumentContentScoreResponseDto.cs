namespace WriteLens.Accessibility.WebAPI.DTOs.Responses;

public class DocumentContentScoreResponseDto
{
    public decimal TotalScore { get; set; }
    public decimal AccessibilityScore { get; set; }
}