namespace WriteLens.Core.WebAPI.DTOs.Document.Responses;

public class DocumentContentSectionResponseDto
{
    public Guid Id { get; set; }

    public decimal OrderIdx { get; set; }

    public string Content { get; set; }
}