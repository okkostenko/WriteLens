using WriteLens.Shared.Models;

namespace WriteLens.Readability.Interfaces.Repositories;

public interface IDocumentContentRepository
{
    public Task<DocumentContent?> GetSingleByIdAsync(Guid documentId);
    public Task MarkDocumentSectionsAsAnalyzedAsync(Guid documentId, List<Guid> sectionsIds);
}