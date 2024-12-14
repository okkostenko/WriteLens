using System.ComponentModel.DataAnnotations;

namespace WriteLens.Core.WebAPI.DTOs.Document.Requests;

public class UpdateDocumentSectionRequestDto
{
    public Guid? Id { get; set; }

    [Required]
    public decimal OrderIdx { get; set; }

    [Required]
    public string Content { get; set; }
}