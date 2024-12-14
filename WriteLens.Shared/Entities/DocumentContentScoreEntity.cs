using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WriteLens.Shared.Entities;

public class DocumentContentScoreEntity
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    [BsonElement("document_id")]
    public Guid DocumentId { get; set; }

    [Required]
    [BsonElement("document_score")]
    public DocumentContentDocumentScoreEntity DocumentScore { get; set; }

    [Required]
    [BsonElement("sections_scores")]
    public List<DocumentContentSectionScoreEntity> SectionsScores { get; set; }
}