namespace WriteLens.Core.WebAPI.DTOs.Document.Responses;

/// <summary>
/// Represents the document's content.
/// </summary>
public class DocumentContentResponseDto
{
    /// <summary>
    /// List of the content sections.
    /// </summary>
    public List<DocumentContentSectionResponseDto>? Sections { get; set; }
}