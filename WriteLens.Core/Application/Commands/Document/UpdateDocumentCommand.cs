namespace WriteLens.Core.Application.Commands.Document;

public record struct UpdateDocumentCommand(string? Title, List<UpdateSectionCommand>? Sections);