using WriteLens.Core.Interfaces.Repositories;
using MongoDB.Driver;
using WriteLens.Shared.Entities;
using Microsoft.Extensions.Options;
using WriteLens.Shared.Settings;
using WriteLens.Shared.Constants;
using Microsoft.AspNetCore.Diagnostics;

namespace WriteLens.Core.Infrastructure.Repositories;

public class MongoDbDocumentScoreRepositoryUpdater : IDocumentScoreRepositoryUpdater
{
    private readonly IMongoCollection<DocumentContentScoreEntity> _documentsContentScoreCollection;
    private readonly FilterDefinitionBuilder<DocumentContentScoreEntity> _filterBuilder =
        Builders<DocumentContentScoreEntity>.Filter;
    
    private readonly UpdateDefinitionBuilder<DocumentContentScoreEntity> _updateBuilder =
        Builders<DocumentContentScoreEntity>.Update;


    public MongoDbDocumentScoreRepositoryUpdater (
        IOptions<MongoDbSettings> mongoDbSettingsOptions,
        IMongoClient mongoClient
    )
    {
        IMongoDatabase database = mongoClient.GetDatabase(
            mongoDbSettingsOptions.Value.DatabaseName);

        _documentsContentScoreCollection = database
            .GetCollection<DocumentContentScoreEntity>(
                MongoDbCollections.DocumentsScores.ToString());
    }

    public async Task PullSectionsScoresBySectionsIdsAsync(Guid documentId, List<Guid> sectionsIds)
    {
        var filter = _filterBuilder.Eq(s => s.DocumentId, documentId);
        var update = _updateBuilder.PullFilter(
            d => d.SectionsScores,
            Builders<DocumentContentSectionScoreEntity>.Filter.In(
                s => s.SectionId, sectionsIds)
        );

        await _documentsContentScoreCollection.UpdateOneAsync(filter, update);
    }

    public async Task PushSectionsScoresAsync(Guid documentId, List<Guid> sectionsIds)
    {
        var createdSections = sectionsIds
            .Select(
                id => new DocumentContentSectionScoreEntity { SectionId = id})
            .ToList();
        
        var filter = _filterBuilder.Eq(s => s.DocumentId, documentId);
        var update = _updateBuilder.PushEach(s => s.SectionsScores, createdSections);

        await _documentsContentScoreCollection.UpdateOneAsync(filter, update);
    }
}