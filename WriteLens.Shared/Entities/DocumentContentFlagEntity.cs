using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WriteLens.Shared.Types;

namespace WriteLens.Shared.Entities;

public class DocumentContentFlagEntity
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [BsonElement("document_id")]
    public Guid DocumentId { get; set; }

    [Required]
    [BsonElement("section_id")]
    public Guid SectionId { get; set; }

    [Required]
    [BsonRepresentation(BsonType.String)]
    [BsonElement("type")]
    public DocumentContentFlagType Type { get; set; }

    [Required]
    [BsonRepresentation(BsonType.String)]
    [BsonElement("severity")]
    public DocumentContentFlagSeverity Severity { get; set; }

    [Required]
    [BsonElement("position")]
    public DocumentContentFlagPositionEntity Position { get; set; }

    [Required]
    [BsonElement("suggestion")]
    public DocumentContentFlagSuggestionEntity? Suggestion { get; set; }

    [Required]
    [BsonElement("created_at")]
    public DateTimeOffset CreatedAt { get; set; }
}