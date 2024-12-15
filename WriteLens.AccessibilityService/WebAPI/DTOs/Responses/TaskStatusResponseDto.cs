namespace WriteLens.Accessibility.WebAPI.DTOs.Responses;

/// <summary>
/// Represents the status of the analysis.
/// </summary>
public class TaskStatusResponseDto
{
    /// <summary>
    /// The ID of the analysis task.
    /// </summary>
    public Guid TaskId { get; set; }

    /// <summary>
    /// The status of the analysis task.
    /// </summary>
    /// <value>Possible values: Pending | Processing | Success | Failed</value>
    public string Status { get; set; } = "Success";

    /// <summary>
    /// [Optional] HTTP status code of the analysis task.
    /// Will be null if the task status is 'Pending' or 'Processing'.
    /// </summary>
    /// <value>Possible values: null | 200 | 400 | 500</value>
    public int? StatusCode { get; set; } = 200;

    /// <summary>
    /// [Optional] The message of the error if occured.
    /// </summary>
    public string? ErrorMessage { get; set; }
}