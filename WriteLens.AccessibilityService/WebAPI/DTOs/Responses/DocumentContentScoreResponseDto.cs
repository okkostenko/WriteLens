namespace WriteLens.Accessibility.WebAPI.DTOs.Responses;

/// <summary>
/// Represent the score of the document content.
/// </summary>
public class DocumentContentScoreResponseDto
{
    /// <summary>
    /// The total score of the document content.
    /// </summary>
    /// <value>Value ranges between 0 and 100.</value>
    public decimal TotalScore { get; set; }

    /// <summary>
    /// The accessibility score of the document content.
    /// </summary>
    /// <value>Value ranges between 0 and 100.</value>
    public decimal AccessibilityScore { get; set; }
}