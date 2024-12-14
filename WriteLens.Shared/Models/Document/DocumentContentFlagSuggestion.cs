namespace WriteLens.Shared.Models;

public class DocumentContentFlagSuggestion
{
    public string Text { get; set; }
    public string? OldText { get; set; }
    public bool IsApplied { get; set; }
    public DateTimeOffset AppliedAt { get; set; }
}