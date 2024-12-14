namespace WriteLens.Core.WebAPI.DTOs.Document.Requests;

/// <summary>
/// Request to update document a document's metadata.
/// </summary>
public class UpdateDocumentRequestDto
{
    /// <summary>
    /// The new title of the document.
    /// </summary>
    public string Title { get; set; }
}