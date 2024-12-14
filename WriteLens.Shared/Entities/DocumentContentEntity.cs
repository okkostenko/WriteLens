using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WriteLens.Shared.Entities;

public class DocumentContentEntity
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    [BsonElement("type_id")]
    public Guid TypeId { get; set; }
    
    [Required]
    [BsonElement("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [BsonElement("sections")]
    public List<DocumentContentSectionEntity>? Sections { get; set; }
}