using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WriteLens.Accessibility.Interfaces.Repositories;
using WriteLens.Shared.Constants;
using WriteLens.Shared.Entities;
using WriteLens.Shared.Models;
using WriteLens.Shared.Settings;

namespace WriteLens.Accessibility.Infrastructure.Repositories;


public class MongoDbDocumentTypeRepository : IDocumentTypeRepository
{
    private readonly IMongoCollection<DocumentTypeEntity> _documentTypesCollection;
    private readonly IMapper _mapper;

    public MongoDbDocumentTypeRepository(IOptions<MongoDbSettings> mongoDbSettingsOptions, IMongoClient mongoClient, IMapper mapper)
    {
        IMongoDatabase database = mongoClient.GetDatabase(mongoDbSettingsOptions.Value.DatabaseName);
        _documentTypesCollection = database.GetCollection<DocumentTypeEntity>(MongoDbCollections.DocumentTypes.ToString());
        _mapper = mapper;
    }

    public async Task<List<DocumentType>> GetManyAsync()
    {
        List<DocumentTypeEntity>? documentTypes = await _documentTypesCollection.Find(_ => true).ToListAsync();
        return documentTypes.Select(_mapper.Map<DocumentType>).ToList();
    }
}