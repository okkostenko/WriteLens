using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WriteLens.Readability.Interfaces.Repositories;
using WriteLens.Shared.Constants;
using WriteLens.Shared.Entities;
using WriteLens.Shared.Models;
using WriteLens.Shared.Settings;

namespace WriteLens.Readability.Infrastructure.Repositories;

public class MongoDbDocumentScoreRepository : IDocumentScoreRepository
{
    private readonly IMongoCollection<DocumentContentScoreEntity> _documentScoreCollection;
    private readonly FilterDefinitionBuilder<DocumentContentScoreEntity> _filterBuilder = Builders<DocumentContentScoreEntity>.Filter;
    private readonly UpdateDefinitionBuilder<DocumentContentScoreEntity> _updateBuilder = Builders<DocumentContentScoreEntity>.Update;
    private readonly IMapper _mapper;

    public MongoDbDocumentScoreRepository(
        IOptions<MongoDbSettings> mongoDbSettingsOptions,
        IMongoClient mongoClient,
        IMapper mapper)
    {
        IMongoDatabase database = mongoClient.GetDatabase(mongoDbSettingsOptions.Value.DatabaseName);
        _documentScoreCollection = database.GetCollection<DocumentContentScoreEntity>(MongoDbCollections.DocumentsScores.ToString());
        _mapper = mapper;
    }

    public async Task<DocumentContentScore?> GetSingleByDocumentIdAsync(Guid documentId)
    {
        var filter = _filterBuilder.Eq(s => s.DocumentId, documentId);
        DocumentContentScoreEntity? documentScore = await _documentScoreCollection.Find(filter).FirstOrDefaultAsync();
        return _mapper.Map<DocumentContentScore>(documentScore);
    }

    public async Task InsertSingleAsync(DocumentContentScore documentContent)
    {
        await _documentScoreCollection.InsertOneAsync(
            _mapper.Map<DocumentContentScoreEntity>(documentContent)
        );
    }

    public async Task UpdateSingleByDocumentIdAsync(
        Guid documentId,
        DocumentContentScore updateDocumentContentScore)
    {
        var filter = _filterBuilder.Eq(s => s.DocumentId, documentId);

        var updateDefinition = _updateBuilder
        .Set(
            s => s.DocumentScore.TotalScore,
            updateDocumentContentScore.DocumentScore.TotalScore)
        .Set(
            s => s.DocumentScore.ReadabilityScore,
            updateDocumentContentScore.DocumentScore.ReadabilityScore)
        .Set(
            s => s.DocumentScore.LastUpdated,
            DateTimeOffset.UtcNow)
        .Set(
            s => s.SectionsScores,
            updateDocumentContentScore.SectionsScores
                .Select(_mapper.Map<DocumentContentSectionScoreEntity>)
        );

        await _documentScoreCollection.UpdateOneAsync(filter, updateDefinition);
    }
}