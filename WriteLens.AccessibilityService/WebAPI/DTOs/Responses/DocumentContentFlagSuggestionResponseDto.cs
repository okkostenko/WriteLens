namespace WriteLens.Accessibility.WebAPI.DTOs.Responses;

/// <summary>
/// Represents the suggestion to improve the flagged text.
/// </summary>
public class DocumentContentFlagSuggestionResponseDto
{
    /// <summary>
    /// The text of the suggestion.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// [Optional] The flagged text if suggestion was applied.
    /// </summary>
    public string? OldText { get; set; }

    /// <summary>
    /// Whether the suggestion was applied or not.
    /// </summary>
    public bool IsApplied { get; set; }

    /// <summary>
    /// The date and time when the suggestion was applied if so.
    /// </summary>
    public DateTimeOffset AppliedAt { get; set; }
}