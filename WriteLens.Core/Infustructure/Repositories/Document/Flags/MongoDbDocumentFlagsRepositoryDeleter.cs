using WriteLens.Core.Interfaces.Repositories;
using MongoDB.Driver;
using WriteLens.Shared.Entities;
using Microsoft.Extensions.Options;
using WriteLens.Shared.Settings;
using WriteLens.Shared.Constants;

namespace WriteLens.Core.Infrastructure.Repositories;

public class MongoDbDocumentFlagsRepositoryDeleter : IDocumentFlagsRepositoryDeleter
{
    private readonly IMongoCollection<DocumentContentFlagEntity> _documentsContentFlagsCollection;
    private readonly FilterDefinitionBuilder<DocumentContentFlagEntity> _filterBuilder =
        Builders<DocumentContentFlagEntity>.Filter;


    public MongoDbDocumentFlagsRepositoryDeleter (
        IOptions<MongoDbSettings> mongoDbSettingsOptions,
        IMongoClient mongoClient
    )
    {
        IMongoDatabase database = mongoClient.GetDatabase(
            mongoDbSettingsOptions.Value.DatabaseName);

        _documentsContentFlagsCollection = database
            .GetCollection<DocumentContentFlagEntity>(
                MongoDbCollections.DocumentFlags.ToString());
    }

    public async Task DeleteManyByDocumentIdAsync(Guid documentId)
    {
        await _documentsContentFlagsCollection.DeleteManyAsync(
            _filterBuilder.Eq(f => f.DocumentId, documentId)
        );
    }

    public async Task DeleteManyBySectionIdAsync(Guid documentId, Guid sectionId)
    {
        await _documentsContentFlagsCollection.DeleteManyAsync(
            _filterBuilder.And(
                _filterBuilder.Eq(f => f.DocumentId, documentId),
                _filterBuilder.Eq(f => f.SectionId, sectionId)
            )
        );
    }

    public async Task DeleteManyBySectionIdAsync(Guid documentId, List<Guid> sectionsId)
    {
        await _documentsContentFlagsCollection.DeleteManyAsync(
            _filterBuilder.And(
                _filterBuilder.Eq(f => f.DocumentId, documentId),
                _filterBuilder.In(f => f.SectionId, sectionsId)
            )
        );
    }
}