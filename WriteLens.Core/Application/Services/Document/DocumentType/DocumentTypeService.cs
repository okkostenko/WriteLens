using WriteLens.Core.Interfaces.Caching;
using WriteLens.Core.Interfaces.Services;
using WriteLens.Shared.Models;

namespace WriteLens.Core.Application.Services;

public class DocumentTypeService : IDocumentTypeService
{
    private readonly IDocumentTypeCache _documentTypeCache;
    
    public DocumentTypeService(IDocumentTypeCache documentTypeCache)
    {
        _documentTypeCache = documentTypeCache;
    }

    public async Task<List<DocumentType>?> GetAllAsync()
    {
        return await _documentTypeCache.GetManyDocumentTypesAsync();
    }
}