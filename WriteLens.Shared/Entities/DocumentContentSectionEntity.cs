using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WriteLens.Shared.Entities;

public class DocumentContentSectionEntity
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [BsonElement("order_idx")]
    public decimal OrderIdx { get; set; }

    [Required]
    [BsonElement("content")]
    public string Content { get; set; }

    [Required]
    [BsonElement("hash")]
    public string Hash { get; set; }

    [Required]
    [BsonElement("is_readability_analyzed")]
    public bool IsReadabilityAnalyzed { get; set; }
    
    [Required]
    [BsonElement("is_accessibility_analyzed")]
    public bool IsAccessibilityAnalyzed { get; set; }
}