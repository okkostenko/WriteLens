using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using WriteLens.Accessibility.Interfaces.Repositories;
using WriteLens.Shared.Constants;
using WriteLens.Shared.Entities;
using WriteLens.Shared.Models;
using WriteLens.Shared.Settings;

namespace WriteLens.Accessibility.Infrastructure.Repositories;

public class MongoDbDocumentContentRepository : IDocumentContentRepository
{
    private readonly IMongoCollection<DocumentContentEntity> _documentContentCollection;
    private readonly FilterDefinitionBuilder<DocumentContentEntity> _filterBuilder = Builders<DocumentContentEntity>.Filter;
    private readonly UpdateDefinitionBuilder<DocumentContentEntity> _updateBuilder = Builders<DocumentContentEntity>.Update;
    private readonly IMapper _mapper;

    public MongoDbDocumentContentRepository(IOptions<MongoDbSettings> mongoDbSettingsOptions, IMongoClient mongoClient, IMapper mapper)
    {
        IMongoDatabase database = mongoClient.GetDatabase(mongoDbSettingsOptions.Value.DatabaseName);
        _documentContentCollection = database.GetCollection<DocumentContentEntity>(MongoDbCollections.DocumentsContent.ToString());
        _mapper = mapper;
    }
    
    public async Task<DocumentContent?> GetSingleByIdAsync(Guid documentId)
    {
        var filter = _filterBuilder.Eq(d => d.Id, documentId);
        DocumentContentEntity? documentContent = await _documentContentCollection.Find(filter).FirstOrDefaultAsync();
        return _mapper.Map<DocumentContent>(documentContent);
    }


    public async Task MarkDocumentSectionsAsAnalyzedAsync(Guid documentId, List<Guid> sectionsIds)
    {
        var filter = _filterBuilder.And(
            _filterBuilder.Eq(d => d.Id, documentId),
            _filterBuilder.ElemMatch(x => x.Sections, s => sectionsIds.Contains(s.Id))
        );

        var update = _updateBuilder.Set(doc => doc.Sections.AllElements().IsAccessibilityAnalyzed, true);
        await _documentContentCollection.UpdateOneAsync(filter, update);
    }
}