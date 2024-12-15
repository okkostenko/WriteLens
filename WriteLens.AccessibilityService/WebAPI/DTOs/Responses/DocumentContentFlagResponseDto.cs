using WriteLens.Shared.Types;

namespace WriteLens.Accessibility.WebAPI.DTOs.Responses;

/// <summary>
/// Represents the accessiblity rules violation flag.
/// </summary>
public class DocumentContentFlagResponseDto
{
    /// <summary>
    /// The ID of the flag.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The type of the flag.
    /// </summary>
    public DocumentContentFlagType Type { get; set; }

    /// <summary>
    /// The severity of the violation.
    /// </summary>
    public DocumentContentFlagSeverity Severity { get; set; }

    /// <summary>
    /// The position of the flag in the section's content.
    /// </summary>
    public DocumentContentFlagPositionResponseDto Position { get; set; }

    /// <summary>
    /// The suggestion of how to improve the flagged text.
    /// </summary>
    public DocumentContentFlagSuggestionResponseDto? Suggestion { get; set; }

    /// <summary>
    /// The date and time the flag was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }
}