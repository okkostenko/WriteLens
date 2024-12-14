using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace WriteLens.Shared.Entities;

public class DocumentContentSectionScoreEntity
{
    [Required]
    [BsonElement("section_id")]
    public Guid SectionId { get; set; }

    [BsonElement("readability_score")]
    public decimal? ReadabilityScore { get; set; }

    [BsonElement("accessibility_score")]
    public decimal? AccessibilityScore { get; set; }

    [BsonElement("last_updated")]
    public DateTimeOffset? LastUpdated { get; set; }   
}