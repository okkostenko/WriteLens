namespace WriteLens.Readability.WebAPI.DTOs.Responses;

public class ReadabilityAnalysisResponseDto
{
    public Guid TaskId { get; set; }
    public string Status { get; set; } = "Success";
    public int StatusCode { get; set; } = 200;
    public string? ErrorMessage { get; set; }
    public DateTimeOffset ProcessedAt { get; set; }
    public ReadabilityAnalysisResultResponseDto? Result { get; set; }
}