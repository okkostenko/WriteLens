namespace WriteLens.Accessibility.WebAPI.DTOs.Responses;

/// <summary>
/// Represets the position of the flagged content
/// relatively to the section the flag belongs to.
/// </summary>
public class DocumentContentFlagPositionResponseDto
{
    /// <summary>
    /// The start index of the flagged content in the section's content string.
    /// </summary>
    public int Start { get; set; }

    /// <summary>
    /// The end index of the flagged content in the section's content string.
    /// </summary>
    /// <remarks>
    /// The carachter in the end index should not be included.
    /// </remarks>
    public int End { get; set; }
}