namespace WriteLens.Core.WebAPI.DTOs.Document.Responses;

/// <summary>
/// Represents the document content seciton.
/// </summary>
public class DocumentContentSectionResponseDto
{
    /// <summary>
    /// The ID of the section.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The positional index of the section.
    /// </summary>
    public decimal OrderIdx { get; set; }

    /// <summary>
    /// The content of the section.
    /// </summary>
    public string Content { get; set; }
}