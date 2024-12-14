using WriteLens.Core.Models.Types;

namespace WriteLens.Core.Application.Commands.Document;

public record struct UpdateSectionCommand(
    Guid? Id,
    decimal OrderIdx,
    string Content,
    string? Hash,
    bool? IsReadabilityAnalyzed,
    bool? IsAccessibilityAnalyzed,
    DocumentContentSectionState? State
);