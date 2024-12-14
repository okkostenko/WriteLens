using System.ComponentModel.DataAnnotations;

namespace WriteLens.Core.WebAPI.DTOs.Document.Requests;

/// <summary>
/// Request to create a new document.
/// </summary>
/// <remarks>
/// All the fields of the request are REQUIRED.
/// </remarks>
public class CreateDocumentRequestDto
{
    /// <summary>
    /// The title of the new document.
    /// </summary>
    /// <value>Untitled Document</value>
    [Required]
    public string Title { get; set; } = "Untitled Document";

    /// <summary>
    /// The ID of the type of the new document.
    /// </summary>
    /// <remarks>
    /// NOTE: The ID of the type must match the ID of one of the available types.
    /// </remarks>
    [Required]
    public Guid TypeId { get; set; }
}