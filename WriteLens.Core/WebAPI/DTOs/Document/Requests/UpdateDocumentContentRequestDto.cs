using System.ComponentModel.DataAnnotations;

namespace WriteLens.Core.WebAPI.DTOs.Document.Requests;

/// <summary>
/// Request to update a document content.
/// </summary>
public class UpdateDocumentContentRequestDto
{
    /// <summary>
    /// A list of the document's sections.
    /// </summary>
    /// <remarks>
    /// Think of the sections of identifiable document paragraphs.
    /// </remarks>
    public List<UpdateDocumentSectionRequestDto> Sections { get; set; }
}