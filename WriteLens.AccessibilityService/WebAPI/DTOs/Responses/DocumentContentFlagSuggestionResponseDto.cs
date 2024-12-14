namespace WriteLens.Accessibility.WebAPI.DTOs.Responses;

public class DocumentContentFlagSuggestionResponseDto
{
    public string Text { get; set; }
    public string? OldText { get; set; }
    public bool IsApplied { get; set; }
    public DateTimeOffset AppliedAt { get; set; }
}