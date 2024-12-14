using System.ComponentModel.DataAnnotations;

namespace WriteLens.Core.WebAPI.DTOs.Document.Requests;

public class CreateDocumentRequestDto
{
    [Required]
    public string Title { get; set; }

    [Required]
    public Guid TypeId { get; set; }
}