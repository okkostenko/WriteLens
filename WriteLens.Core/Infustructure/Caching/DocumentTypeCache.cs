using Microsoft.Extensions.Caching.Memory;
using WriteLens.Core.Interfaces.Caching;
using WriteLens.Core.Interfaces.Repositories;
using WriteLens.Shared.Models;
using ZstdSharp.Unsafe;

namespace WriteLens.Core.Infrastructure.Caching;

public class DocumentTypeCache : IDocumentTypeCache
{
    private const string CACHE_KEY = "DocumentTypes";
    private readonly IMemoryCache _cache;
    private readonly IDocumentTypeRepository _documentTypeRepository;

    public DocumentTypeCache(IMemoryCache cache, IDocumentTypeRepository documentTypeRepository)
    {
        _cache = cache;
        _documentTypeRepository = documentTypeRepository;
    }

    public void InvalidateCache()
    {
        _cache.Remove(CACHE_KEY);
    }

    public async Task RefreshCacheAsync()
    {
        await loadDocumentTypesFromDB();
    }

    public async Task<DocumentType?> GetDocumentTypeByIdAsync(Guid typeId)
    {
        if (_cache.TryGetValue(CACHE_KEY, out Dictionary<Guid, DocumentType>? documentTypes))
        {
            if (documentTypes is null)
                return null;
    
            documentTypes.TryGetValue(typeId, out var documentType);
            return await Task.FromResult(documentType);
        }
        return null;
    }

    public async Task<List<DocumentType>?> GetManyDocumentTypesAsync()
    {
        Dictionary<Guid, DocumentType>? documentTypes;
        if ( ! _cache.TryGetValue(CACHE_KEY, out documentTypes))
        {
            documentTypes = await loadDocumentTypesFromDB();
        }

        if (documentTypes is null)
            return new List<DocumentType>();

        return await Task.FromResult(documentTypes.Values.ToList());
    }

    private async Task<Dictionary<Guid, DocumentType>?> loadDocumentTypesFromDB()
    {
        var documentTypes = await _documentTypeRepository.GetManyAsync();
        var documentTypesDictionary = documentTypes.ToDictionary(dt => dt.Id);

        _cache.Set(CACHE_KEY, documentTypesDictionary);
        return documentTypesDictionary;
    }
}