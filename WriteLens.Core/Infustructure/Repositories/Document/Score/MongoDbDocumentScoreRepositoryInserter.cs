using WriteLens.Shared.Models;
using WriteLens.Core.Interfaces.Repositories;
using WriteLens.Core.Application.Commands.Document;
using WriteLens.Core.Models.DomainModels.Document;
using MongoDB.Driver;
using WriteLens.Shared.Entities;
using AutoMapper;
using WriteLens.Shared.Settings;
using Microsoft.Extensions.Options;
using WriteLens.Shared.Constants;

namespace WriteLens.Core.Infrastructure.Repositories;

public class MongoDbDocumentScoreRepositoryInserter : IDocumentScoreRepositoryInserter
{
    private readonly IMongoCollection<DocumentContentScoreEntity> _documentContentScoresCollection;
    private readonly IMapper _mapper;

    public MongoDbDocumentScoreRepositoryInserter (
        IOptions<MongoDbSettings> mongoDbSettingsOptions,
        IMongoClient mongoClient,
        IMapper mapper
    )
    {
        IMongoDatabase database = mongoClient.GetDatabase(
            mongoDbSettingsOptions.Value.DatabaseName);

        _documentContentScoresCollection = database
            .GetCollection<DocumentContentScoreEntity>(
                MongoDbCollections.DocumentsScores.ToString());

        _mapper = mapper;
    }

    public async Task InsertSingleAsync(DocumentContentScore score)
    {
        await _documentContentScoresCollection.InsertOneAsync(
            _mapper.Map<DocumentContentScoreEntity>(score));
    }
}