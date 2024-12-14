namespace WriteLens.Core.WebAPI.DTOs.Document.Responses;

/// <summary>
/// Represent a document type.
/// </summary>
public class DocumentTypeResponseDto
{
    /// <summary>
    /// The ID of the type.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// The name of the type.
    /// </summary>
    public string TypeName { get; set; }

    /// <summary>
    /// The description of the type.
    /// </summary>
    public string Desctiprion { get; set; }
}