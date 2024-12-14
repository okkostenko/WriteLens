using WriteLens.Shared.Types;

namespace WriteLens.Shared.Models;

public class DocumentContentFlag
{
    public Guid Id { get; set; }

    public Guid DocumentId { get; set; }

    public Guid SectionId { get; set; }

    public DocumentContentFlagType Type { get; set; }

    public DocumentContentFlagSeverity Severity { get; set; }

    public DocumentContentFlagPosition Position { get; set; }

    public DocumentContentFlagSuggestion? Suggestion { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}