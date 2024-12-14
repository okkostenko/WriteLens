using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace WriteLens.Shared.Entities;

public class DocumentContentFlagPositionEntity
{
    [Required]
    [BsonElement("start")]
    public int Start { get; set; }

    [Required]
    [BsonElement("end")]
    public int End { get; set; }
}