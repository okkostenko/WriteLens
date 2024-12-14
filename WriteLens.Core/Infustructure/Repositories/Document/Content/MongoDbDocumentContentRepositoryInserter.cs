using WriteLens.Shared.Models;
using WriteLens.Shared.Entities;
using WriteLens.Shared.Settings;
using WriteLens.Shared.Constants;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using AutoMapper;
using WriteLens.Core.Interfaces.Repositories;

namespace WriteLens.Core.Infrastructure.Repositories;

public class MongoDbDocumentContentRepositoryInserter : IDocumentContentRepositoryInserter
{
    private readonly IMongoCollection<DocumentContentEntity> _documentsContentCollection;
    private readonly IMapper _mapper;

    public MongoDbDocumentContentRepositoryInserter(IOptions<MongoDbSettings> mongoDbSettingsOptions, IMongoClient mongoClient, IMapper mapper)
    {
        IMongoDatabase database = mongoClient.GetDatabase(mongoDbSettingsOptions.Value.DatabaseName);
        _documentsContentCollection = database.GetCollection<DocumentContentEntity>(MongoDbCollections.DocumentsContent.ToString());
        _mapper = mapper;
    }

    public async Task InsertSingleAsync(DocumentContent document)
    {
        await _documentsContentCollection.InsertOneAsync(_mapper.Map<DocumentContentEntity>(document));
    }
}