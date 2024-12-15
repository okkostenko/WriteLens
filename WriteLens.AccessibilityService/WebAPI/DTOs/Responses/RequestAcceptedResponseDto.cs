namespace WriteLens.Accessibility.WebAPI.DTOs.Responses;

/// <summary>
/// Represent the ID of the analysis task.
/// </summary>
public class RequestAcceptedResponseDto
{
    /// <summary>
    /// The ID of the analysis task.
    /// </summary>
    public Guid TaskId { get; set; }
}