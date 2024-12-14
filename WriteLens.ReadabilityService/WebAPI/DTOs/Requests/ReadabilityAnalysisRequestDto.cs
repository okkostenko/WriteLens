namespace WriteLens.Readability.WebAPI.DTOs.Requests;

public class ReadabilityAnalysisRequestDto
{
    public Guid TaskId { get; set; } = Guid.NewGuid();
    public Guid DocumentId { get; set; }
}