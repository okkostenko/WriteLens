using WriteLens.Shared.Models;

namespace WriteLens.Shared.Interfaces.Caching;

public interface IDocumentTypeCache
{
    Task<DocumentType?> GetDocumentTypeByIdAsync(Guid typeId);
    Task<List<DocumentType>?> GetManyDocumentTypesAsync();
    Task RefreshCacheAsync();
    void InvalidateCache();
}