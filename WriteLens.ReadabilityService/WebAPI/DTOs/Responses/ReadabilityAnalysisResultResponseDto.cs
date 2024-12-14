namespace WriteLens.Readability.WebAPI.DTOs.Responses;

public class ReadabilityAnalysisResultResponseDto
{
    public decimal TotalScore { get; set; }
    public decimal ReadabilityScore { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
}