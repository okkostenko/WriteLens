namespace WriteLens.Readability.WebAPI.DTOs.Responses;

public class TaskStatusResponseDto
{
    public Guid TaskId { get; set; }
    public string Status { get; set; } = "Success";
    public int? StatusCode { get; set; } = 200;
    public string? ErrorMessage { get; set; }
}