namespace WriteLens.Core.WebAPI.DTOs.Document.Responses;

public class DocumentListItemResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DocumentTypeResponseDto Type {get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}