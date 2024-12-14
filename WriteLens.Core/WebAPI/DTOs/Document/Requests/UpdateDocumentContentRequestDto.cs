using System.ComponentModel.DataAnnotations;

namespace WriteLens.Core.WebAPI.DTOs.Document.Requests;

public class UpdateDocumentContentRequestDto
{
    public List<UpdateDocumentSectionRequestDto> Sections { get; set; }
}