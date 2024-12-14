using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace WriteLens.Shared.Entities;

public class DocumentContentFlagSuggestionEntity
{
    [Required]
    [BsonElement("text")]
    public string Text { get; set; }

    [BsonElement("old_text")]
    public string? OldText { get; set; }

    [BsonElement("is_applied")]
    public bool IsApplied { get; set; }

    [BsonElement("applied_at")]
    public DateTimeOffset AppliedAt { get; set; }
}