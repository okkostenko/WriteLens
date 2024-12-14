using System.ComponentModel.DataAnnotations;

namespace WriteLens.Core.WebAPI.DTOs.Document.Requests;

/// <summary>
/// Request to update the document section.
/// </summary>
public class UpdateDocumentSectionRequestDto
{
    /// <summary>
    /// The ID of the section.
    /// </summary>
    /// <remarks>
    /// If the section is already exists and needs to be updated, provide it's ID.
    /// Otherwise, set value to null for the section to be created.
    /// </remarks>
    public Guid? Id { get; set; }

    /// <summary>
    /// The positional index of the section.
    /// </summary>
    /// <remarks>
    /// The positional index is represented by decimal value.
    /// 
    /// To place the section in between two other sections,
    /// provide such value so that it's in between the desirable sections's order indexes.
    ///
    /// Exmaple:
    /// Section1.OrderIdx = 1;
    /// Section2.OrderIdx = 2;
    /// SectionToPlaceInBetween.OrderIdx = 1.4
    /// </remarks>
    [Required]
    public decimal OrderIdx { get; set; }

    /// <summary>
    /// The new text content of the section.
    /// </summary>
    [Required]
    public string Content { get; set; }
}