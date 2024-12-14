using MongoDB.Bson.Serialization.Attributes;

namespace WriteLens.Shared.Entities;

public class DocumentTypeRulesetEntity
{
    [BsonElement("readability_threshold")]
    public double ReadabilityThreshold { get; set; }

    [BsonElement("complexity_penalty")]
    public double ComplexityPenalty { get; set; }

    [BsonElement("max_sentence_length")]
    public int MaxSentenceLength { get; set; }

    [BsonElement("allow_passive_voice")]
    public bool AllowPassiveVoice { get; set; }

    [BsonElement("allow_jargon")]
    public bool AllowJargon { get; set; }

    [BsonElement("jargon_penalty")]
    public double JargonPenalty { get; set; }

    [BsonElement("passive_voice_penalty")]
    public double PassiveVoicePenalty { get; set; }

    [BsonElement("sentence_complexity_penalty")]
    public double SentenceComplexityPenalty { get; set; }

    [BsonElement("noninclusive_language_penalty")]
    public double NonInclusiveLanguagePenalty { get; set; }

    [BsonElement("max_complexity_score")]
    public double MaxComplexityScore { get; set; }

    [BsonElement("min_section_readability_score")]
    public double MinSectionReadabilityScore { get; set; }

    [BsonElement("max_flags_per_section")]
    public int MaxFlagsPerSection { get; set; }
}