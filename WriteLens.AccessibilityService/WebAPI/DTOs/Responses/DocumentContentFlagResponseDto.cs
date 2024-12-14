using WriteLens.Shared.Types;

namespace WriteLens.Accessibility.WebAPI.DTOs.Responses;

public class DocumentContentFlagResponseDto
{
    public Guid Id { get; set; }
    public DocumentContentFlagType Type { get; set; }
    public DocumentContentFlagSeverity Severity { get; set; }
    public DocumentContentFlagPositionResponseDto Position { get; set; }
    public DocumentContentFlagSuggestionResponseDto? Suggestion { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}