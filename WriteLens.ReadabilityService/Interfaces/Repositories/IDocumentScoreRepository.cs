using WriteLens.Shared.Models;

namespace WriteLens.Readability.Interfaces.Repositories;

public interface IDocumentScoreRepository
{
    public Task<DocumentContentScore?> GetSingleByDocumentIdAsync(Guid documentId);
    public Task InsertSingleAsync(DocumentContentScore documentContent);
    public Task UpdateSingleByDocumentIdAsync(
        Guid documentId,
        DocumentContentScore updateDocumentContentScore);
}