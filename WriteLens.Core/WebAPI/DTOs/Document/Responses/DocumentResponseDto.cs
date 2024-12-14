namespace WriteLens.Core.WebAPI.DTOs.Document.Responses;

public class DocumentResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public DocumentTypeResponseDto Type {get; set; }
    public DocumentContentResponseDto? Content { get; set; }
    public List<DocumentContentFlagResponseDto>? Flags { get; set; }
    public DocumentContentScoreResponseDto? Score { get; set; }
}