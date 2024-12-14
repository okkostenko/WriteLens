using WriteLens.Shared.Models;
using WriteLens.Shared.Entities;
using WriteLens.Shared.Settings;
using WriteLens.Shared.Constants;
using WriteLens.Core.Interfaces.Repositories;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using AutoMapper;
using MongoDB.Bson;
using WriteLens.Core.Infrastructure.Data.MongoDb.Entities;
using WriteLens.Core.Models.DomainModels.Document;

namespace WriteLens.Core.Infrastructure.Repositories;

public class MongoDbDocumentContentRepositoryRetriever : IDocumentContentRepositoryRetriever
{
    private readonly IMongoCollection<DocumentContentEntity> _documentsContentCollection;
    private readonly IMapper _mapper;

    public MongoDbDocumentContentRepositoryRetriever(IOptions<MongoDbSettings> mongoDbSettingsOptions, IMongoClient mongoClient, IMapper mapper)
    {
        IMongoDatabase database = mongoClient.GetDatabase(mongoDbSettingsOptions.Value.DatabaseName);
        _documentsContentCollection = database.GetCollection<DocumentContentEntity>(MongoDbCollections.DocumentsContent.ToString());
        _mapper = mapper;
    }

    public async Task<List<DocumentContentWithAnalysisData>> GetManyByDocumentsIdsAsync(List<Guid> documentsIds, DocumentQueryParams? queryParams)
    {
        var pipeline = MongoDbDocumentContentAggregationPipelineConstructor.ConstructPipeline(
            new BsonDocument("$match", new BsonDocument
            {
                { "_id", new BsonDocument(
                    "$in",
                    new BsonArray(
                        documentsIds.Select(id => id.ToString())
                    )
                )}
            })
        );
        List<DocumentContentWithAnalysisDataEntity>? documentsContents = await _documentsContentCollection
            .Aggregate<DocumentContentWithAnalysisDataEntity>(pipeline)
            .ToListAsync();
        return documentsContents.Select(_mapper.Map<DocumentContentWithAnalysisData>).ToList();
    }

    public async Task<DocumentContentWithAnalysisData?> GetSingleByIdAsync(Guid documentId)
    {     
        var pipeline = MongoDbDocumentContentAggregationPipelineConstructor.ConstructPipeline(
            new BsonDocument("$match", new BsonDocument
            {
                { "_id", documentId.ToString()}
            })
        );

        DocumentContentWithAnalysisDataEntity? documentContent = await _documentsContentCollection
            .Aggregate<DocumentContentWithAnalysisDataEntity>(pipeline)
            .FirstOrDefaultAsync();
        
        return _mapper.Map<DocumentContentWithAnalysisData>(documentContent);
    }
}