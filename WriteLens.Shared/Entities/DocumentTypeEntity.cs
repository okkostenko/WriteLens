using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WriteLens.Shared.Entities;

public class DocumentTypeEntity
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    [BsonElement("type_name")]
    public string TypeName { get; set; }
    
    [Required]
    [BsonElement("description")]
    public string Desctiprion { get; set; }

    [Required]
    [BsonElement("ruleset")]
    public DocumentTypeRulesetEntity Ruleset { get; set; }
}