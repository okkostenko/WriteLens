using System.Text.Json.Serialization;

namespace WriteLens.Accessibility.Models.ApplicationModels;

public class MLTextAnalysisResultResponse
{
    [JsonPropertyName("task_id")]
    public string TaskId { get; set; }
    public MLTextAnalysisResult Result { get; set; }
}