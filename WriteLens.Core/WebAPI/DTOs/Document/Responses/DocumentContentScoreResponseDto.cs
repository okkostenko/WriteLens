namespace WriteLens.Core.WebAPI.DTOs.Document.Responses;

/// <summary>
/// Represent the score of the document content.
/// </summary>
public class DocumentContentScoreResponseDto
{
    /// <summary>
    /// The total score of the document content.
    /// </summary>
    public decimal TotalScore { get; set; }

    /// <summary>
    /// The readability score of the document content.
    /// </summary>
    public decimal ReadabilityScore { get; set; }

    /// <summary>
    /// The accessibility score of the document content.
    /// </summary>
    public decimal AccessibilityScore { get; set; }
    
    /// <summary>
    /// The date and the time the score was last updated.
    /// </summary>
    public DateTimeOffset LastUpdated { get; set; }
}