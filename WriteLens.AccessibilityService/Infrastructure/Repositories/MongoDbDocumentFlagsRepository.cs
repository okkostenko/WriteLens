using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WriteLens.Accessibility.Interfaces.Repositories;
using WriteLens.Accessibility.Models.Commands;
using WriteLens.Shared.Constants;
using WriteLens.Shared.Entities;
using WriteLens.Shared.Models;
using WriteLens.Shared.Settings;

namespace WriteLens.Accessibility.Infrastructure.Repositories;

public class MongoDbDocumentFlagsRepository : IDocumentFlagsRepository
{
    private readonly IMongoCollection<DocumentContentFlagEntity> _documentFlagsCollection;
    private readonly FilterDefinitionBuilder<DocumentContentFlagEntity> _filterBuilder = Builders<DocumentContentFlagEntity>.Filter;
    private readonly UpdateDefinitionBuilder<DocumentContentFlagEntity> _updateBuilder = Builders<DocumentContentFlagEntity>.Update;
    private readonly IMapper _mapper;
    public MongoDbDocumentFlagsRepository(
        IOptions<MongoDbSettings> mongoDbSettingsOptions,
        IMongoClient mongoClient,
        IMapper mapper)
    {
        IMongoDatabase database = mongoClient.GetDatabase(mongoDbSettingsOptions.Value.DatabaseName);
        _documentFlagsCollection = database.GetCollection<DocumentContentFlagEntity>(MongoDbCollections.DocumentFlags.ToString());
        _mapper = mapper;
    }
    public async Task DeleteManyBySectionIdAsync(Guid sectionId)
    {
        var filter = _filterBuilder.Eq(f => f.SectionId, sectionId);
        await _documentFlagsCollection.DeleteManyAsync(filter);
    }
    public async Task DeleteManyBySectionIdAsync(List<Guid> sectionIds)
    {
        var filter = _filterBuilder.In(f => f.SectionId, sectionIds);
        await _documentFlagsCollection.DeleteManyAsync(filter);
    }

    public async Task DeleteSingleByIdAsync(Guid flagId)
    {
        var filter = _filterBuilder.Eq(f => f.Id, flagId);
        await _documentFlagsCollection.DeleteOneAsync(filter);
    }

    public async Task<List<DocumentContentFlag>?> GetManyByDocumentIdAsync(Guid documentId)
    {
        var filter = _filterBuilder.Eq(f => f.DocumentId, documentId);
        List<DocumentContentFlagEntity>? documentFlags = await _documentFlagsCollection.Find(filter).ToListAsync();
        return _mapper.Map<List<DocumentContentFlag>?>(documentFlags);
    }

    public async Task<List<DocumentContentFlag>?> GetManyBySectionIdAsync(Guid sectionId)
    {
        var filter = _filterBuilder.Eq(f => f.SectionId, sectionId);
        List<DocumentContentFlagEntity>? documentFlags = await _documentFlagsCollection.Find(filter).ToListAsync();
        return _mapper.Map<List<DocumentContentFlag>?>(documentFlags);
    }

    public async Task<DocumentContentFlag?> GetSingleByIdAsync(Guid flagId)
    {
        var filter = _filterBuilder.Eq(f => f.Id, flagId);
        DocumentContentFlagEntity? documentFlag = await _documentFlagsCollection.Find(filter).FirstOrDefaultAsync();
        return _mapper.Map<DocumentContentFlag>(documentFlag);
    }

    public async Task InserManyAsync(List<DocumentContentFlag> documentFlags)
    {
        await _documentFlagsCollection.InsertManyAsync(
            _mapper.Map<List<DocumentContentFlagEntity>>(documentFlags)
        );
    }

    public async Task InsertSingleAsync(DocumentContentFlag documentFlag)
    {
        await _documentFlagsCollection.InsertOneAsync(
            _mapper.Map<DocumentContentFlagEntity>(documentFlag)
        );
    }

    public async Task UpdateSingleById(Guid flagId, UpdateDocumentContentFlagCommand updateFlagCommand)
    {
        var filter = _filterBuilder.Eq(f => f.Id, flagId);
        var updateDefinition = _updateBuilder.Set(
            s => s.Suggestion.IsApplied,
            updateFlagCommand.Suggestion.IsApplied
        );

        await _documentFlagsCollection.UpdateOneAsync(filter, updateDefinition);
    }
}