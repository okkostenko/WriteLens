using System.Text.Json.Serialization;

namespace WriteLens.Accessibility.Models.ApplicationModels;

public class MLTextAnalysisStatus
{
    [JsonPropertyName("status")]
    public string Status { get; set; }
}