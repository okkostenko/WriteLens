namespace WriteLens.Core.WebAPI.DTOs.Document.Responses;

/// <summary>
/// Represents a document.
/// </summary>
public class DocumentResponseDto
{
    /// <summary>
    /// The ID of the document.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The title of the document.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// The date and time of the document creation.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// The date and the time the document was last updated at.
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// The type of the document.
    /// </summary>
    public DocumentTypeResponseDto Type {get; set; }

    /// <summary>
    /// The content of the document if exists.
    /// </summary>
    public DocumentContentResponseDto? Content { get; set; }

    /// <summary>
    /// The list of the accessibility rules violation flags.
    /// </summary>
    public List<DocumentContentFlagResponseDto>? Flags { get; set; }

    /// <summary>
    /// The score of the document content if exists.
    /// </summary>
    public DocumentContentScoreResponseDto? Score { get; set; }
}