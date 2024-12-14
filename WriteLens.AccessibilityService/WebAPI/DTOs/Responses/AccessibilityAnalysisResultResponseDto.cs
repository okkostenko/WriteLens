namespace WriteLens.Accessibility.WebAPI.DTOs.Responses;

public class AccessibilityAnalysisResultResponseDto
{
    public DocumentContentScoreResponseDto Score { get; set; }
    public List<DocumentContentFlagResponseDto>? Flags { get; set; }
}