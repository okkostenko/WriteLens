namespace WriteLens.Core.WebAPI.DTOs.Document.Responses;

/// <summary>
/// Represents a document.
/// </summary>
/// <remarks>
/// NOTE: the model is a PARTIAL representation of the document,
/// that includes the document's metadata and type.
/// </remarks>
public class DocumentListItemResponseDto
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
    /// The type of the document.
    /// </summary>
    public DocumentTypeResponseDto Type {get; set; }

    /// <summary>
    /// The date and the time of the document creation.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }
}