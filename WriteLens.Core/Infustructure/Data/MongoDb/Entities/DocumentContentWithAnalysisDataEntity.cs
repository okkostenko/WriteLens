using MongoDB.Bson.Serialization.Attributes;
using WriteLens.Core.Models.DomainModels.Document;
using WriteLens.Shared.Entities;

namespace WriteLens.Core.Infrastructure.Data.MongoDb.Entities;

public class DocumentContentWithAnalysisDataEntity
{
    [BsonElement("_id")]
    public Guid Id { get; set; }
    
    [BsonElement("type_id")]
    public Guid TypeId { get; set; }
    
    [BsonElement("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [BsonElement("type")]
    public DocumentTypeEntity? Type { get; set; }

    [BsonElement("sections")]
    public List<DocumentContentSectionEntity>? Sections { get; set; }

    [BsonElement("score")]
    public DocumentContentDocumentScoreEntity? Score { get; set; }

    [BsonElement("flags")]
    public List<DocumentContentFlagEntity>? Flags { get; set; }
}