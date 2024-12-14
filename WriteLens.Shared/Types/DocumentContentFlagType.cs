using System.Text.Json.Serialization;

namespace WriteLens.Shared.Types;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DocumentContentFlagType
{
    sentenceComplexity,
    passiveVoice,
    noninclusiveLanguage,
    jargon
}