namespace WriteLens.Core.Application.Commands.Document;

public record struct CreateDocumentCommand(Guid TypeId, string Title);