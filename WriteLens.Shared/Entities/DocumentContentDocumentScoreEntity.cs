using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace WriteLens.Shared.Entities;

public class DocumentContentDocumentScoreEntity
{
    [BsonElement("total_score")]
    public decimal? TotalScore { get; set; }

    [BsonElement("readability_score")]
    public decimal? ReadabilityScore { get; set; }

    [BsonElement("clarity_score")]
    public decimal? ClarityScore { get; set; }

    [BsonElement("accessibility_score")]
    public decimal? AccessibilityScore { get; set; }

    [BsonElement("last_updated")]
    public DateTimeOffset? LastUpdated { get; set; }   
}