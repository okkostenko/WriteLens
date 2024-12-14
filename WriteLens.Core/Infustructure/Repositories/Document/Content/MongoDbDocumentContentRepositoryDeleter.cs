using WriteLens.Shared.Entities;
using WriteLens.Shared.Settings;
using WriteLens.Shared.Constants;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using WriteLens.Core.Interfaces.Repositories;

namespace WriteLens.Core.Infrastructure.Repositories;

public class MongoDbDocumentContentRepositoryDeleter : IDocumentContentRepositoryDeleter
{
    private readonly IMongoCollection<DocumentContentEntity> _documentsContentCollection;
    private readonly IMongoCollection<DocumentContentFlagEntity> _documentsContentFlagsCollection;
    private readonly IMongoCollection<DocumentContentScoreEntity> _documentContentScoresCollection;
    private readonly FilterDefinitionBuilder<DocumentContentEntity> _filterBuilder = Builders<DocumentContentEntity>.Filter;

    public MongoDbDocumentContentRepositoryDeleter(IOptions<MongoDbSettings> mongoDbSettingsOptions, IMongoClient mongoClient)
    {
        IMongoDatabase database = mongoClient.GetDatabase(mongoDbSettingsOptions.Value.DatabaseName);
        _documentsContentCollection = database.GetCollection<DocumentContentEntity>(MongoDbCollections.DocumentsContent.ToString());
        _documentsContentFlagsCollection = database.GetCollection<DocumentContentFlagEntity>(MongoDbCollections.DocumentFlags.ToString());
        _documentContentScoresCollection = database.GetCollection<DocumentContentScoreEntity>(MongoDbCollections.DocumentsScores.ToString());
    }

    public async Task DeleteSingleByIdAsync(Guid documentId)
    {
        var filter = _filterBuilder.Eq(d => d.Id, documentId);

        await Task.WhenAll(
            _documentsContentCollection.DeleteOneAsync(filter),
            _documentContentScoresCollection.DeleteOneAsync(
                Builders<DocumentContentScoreEntity>.Filter.Eq(s => s.DocumentId, documentId)
            ),
            _documentsContentFlagsCollection.DeleteManyAsync(
                Builders<DocumentContentFlagEntity>.Filter.Eq(f => f.DocumentId, documentId)
            )
        );
    }
}