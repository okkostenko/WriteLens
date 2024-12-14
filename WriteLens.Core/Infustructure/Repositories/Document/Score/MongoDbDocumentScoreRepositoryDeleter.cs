using WriteLens.Core.Interfaces.Repositories;
using MongoDB.Driver;
using WriteLens.Shared.Entities;
using Microsoft.Extensions.Options;
using WriteLens.Shared.Settings;
using WriteLens.Shared.Constants;

namespace WriteLens.Core.Infrastructure.Repositories;

public class MongoDbDocumentScoreRepositoryDeleter : IDocumentScoreRepositoryDeleter
{
    private readonly IMongoCollection<DocumentContentFlagEntity> _documentsContentScoreCollection;
    private readonly FilterDefinitionBuilder<DocumentContentFlagEntity> _filterBuilder =
        Builders<DocumentContentFlagEntity>.Filter;


    public MongoDbDocumentScoreRepositoryDeleter (
        IOptions<MongoDbSettings> mongoDbSettingsOptions,
        IMongoClient mongoClient
    )
    {
        IMongoDatabase database = mongoClient.GetDatabase(
            mongoDbSettingsOptions.Value.DatabaseName);

        _documentsContentScoreCollection = database
            .GetCollection<DocumentContentFlagEntity>(
                MongoDbCollections.DocumentsScores.ToString());
    }

    public async Task DeleteSingleByDocumentIdAsync(Guid documentId)
    {
        await _documentsContentScoreCollection.DeleteOneAsync(
            _filterBuilder.Eq(f => f.DocumentId, documentId)
        );
    }
}