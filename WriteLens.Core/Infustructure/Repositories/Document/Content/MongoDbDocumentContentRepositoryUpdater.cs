using WriteLens.Shared.Entities;
using WriteLens.Shared.Settings;
using WriteLens.Shared.Constants;
using WriteLens.Core.Interfaces.Repositories;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using AutoMapper;
using WriteLens.Core.Application.Commands.Document;

namespace WriteLens.Core.Infrastructure.Repositories;

public class MongoDbDocumentContentRepositoryUpdater : IDocumentContentRepositoryUpdater
{
    private readonly IMongoCollection<DocumentContentEntity> _documentsContentCollection;
    private readonly FilterDefinitionBuilder<DocumentContentEntity> _filterBuilder = Builders<DocumentContentEntity>.Filter;
    private readonly UpdateDefinitionBuilder<DocumentContentEntity> _updateBuilder = Builders<DocumentContentEntity>.Update;
    private readonly IMapper _mapper;

    public MongoDbDocumentContentRepositoryUpdater(IOptions<MongoDbSettings> mongoDbSettingsOptions, IMongoClient mongoClient, IMapper mapper)
    {
        IMongoDatabase database = mongoClient.GetDatabase(mongoDbSettingsOptions.Value.DatabaseName);
        _documentsContentCollection = database.GetCollection<DocumentContentEntity>(MongoDbCollections.DocumentsContent.ToString());
        _mapper = mapper;
    }

    public async Task UpdateSingleByIdAsync(Guid documentId, UpdateDocumentCommand updateDocumentCommand)
    {
        if (updateDocumentCommand.Sections is null) return;

        var filter = _filterBuilder.Eq(d => d.Id, documentId);

        var updateDocumentDefinition = _updateBuilder.Set(
            d => d.Sections,
            updateDocumentCommand.Sections.Select(s => _mapper.Map<DocumentContentSectionEntity>(s)).ToList()
        );

        await _documentsContentCollection.UpdateOneAsync(filter, updateDocumentDefinition);
    }
}