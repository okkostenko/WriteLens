using WriteLens.Shared.Constants;
using MongoDB.Bson;

namespace WriteLens.Core.Infrastructure.Repositories;

public static class MongoDbDocumentContentAggregationPipelineConstructor
{
    public static BsonDocument[] ConstructPipeline(BsonDocument matchStage)
    {
        return [
            matchStage,
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", MongoDbCollections.DocumentTypes.ToString() },
                { "localField", "type_id" },
                { "foreignField", "_id" },
                { "as", "type" }
            }),
            new BsonDocument("$unwind", new BsonDocument
            {
                { "path", "$type" },
                { "preserveNullAndEmptyArrays", true }
            }),
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", MongoDbCollections.DocumentsScores.ToString() },
                { "localField", "_id" },
                { "foreignField", "document_id" },
                { "as", "score" }
            }),
            new BsonDocument("$unwind", new BsonDocument
            {
                { "path", "$score" },
                { "preserveNullAndEmptyArrays", true }
            }),
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", MongoDbCollections.DocumentFlags.ToString() },
                { "localField", "_id" },
                { "foreignField", "document_id" },
                { "as", "flags" }
            }),
            new BsonDocument("$addFields", new BsonDocument
            {
                { "sections", new BsonDocument("$sortArray", new BsonDocument
                    {
                        { "input", "$sections" },
                        { "sortBy", new BsonDocument("order_idx", 1) }
                    })
                }
            }),
            new BsonDocument("$project", new BsonDocument
            {
                { "_id", 1 },
                { "type_id", 1 },
                { "type", "$type" },
                { "created_at", 1 },
                { "sections", 1 },
                { "score", "$score.document_score" },
                { "flags", "$flags"}
            })
        ];
    }
}