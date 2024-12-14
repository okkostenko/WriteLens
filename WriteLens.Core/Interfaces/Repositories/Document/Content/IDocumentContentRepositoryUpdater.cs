using WriteLens.Core.Application.Commands.Document;

namespace WriteLens.Core.Interfaces.Repositories;

public interface IDocumentContentRepositoryUpdater
{
    public Task UpdateSingleByIdAsync(Guid documentId, UpdateDocumentCommand updateDocumentCommand);
}