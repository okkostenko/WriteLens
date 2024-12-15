namespace WriteLens.Accessibility.WebAPI.DTOs.Responses;

/// <summary>
/// Represent the result of the analysis.
/// </summary>
public class AccessibilityAnalysisResultResponseDto
{
    /// <summary>
    /// The socre of the document.
    /// </summary>
    public DocumentContentScoreResponseDto Score { get; set; }

    /// <summary>
    /// [Optional ]A list of of accessability rules violation flags if found.
    /// </summary> <summary>
    public List<DocumentContentFlagResponseDto>? Flags { get; set; }
}