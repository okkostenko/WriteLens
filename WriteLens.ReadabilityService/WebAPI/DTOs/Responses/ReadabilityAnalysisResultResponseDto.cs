namespace WriteLens.Readability.WebAPI.DTOs.Responses;

/// <summary>
/// Represents the result of the analysis.
/// </summary>
public class ReadabilityAnalysisResultResponseDto
{
    /// <summary>
    /// The total score of the document's content.
    /// </summary>
    /// <value>Value ranges between 0 and 100.</value>
    public decimal TotalScore { get; set; }

    /// <summary>
    /// The readability score of the document's content.
    /// </summary>
    /// <value>Value ranges between 0 and 100.</value>
    public decimal ReadabilityScore { get; set; }

    /// <summary>
    /// The date and the time the score was last updated at.
    /// </summary>
    public DateTimeOffset LastUpdated { get; set; }
}