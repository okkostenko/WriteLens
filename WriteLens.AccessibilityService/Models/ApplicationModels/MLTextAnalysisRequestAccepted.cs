using System.Text.Json.Serialization;

namespace WriteLens.Accessibility.Models.ApplicationModels;

public class MLTextAnalysisRequestAccepted
{
    [JsonPropertyName("task_id")]
    public string TaskId { get; set; }
}