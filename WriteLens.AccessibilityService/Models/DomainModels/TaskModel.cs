using WriteLens.Accessibility.Models.ApplicationModels;

namespace WriteLens.Accessibility.Models.DomainModels;

public class TaskModel
{
    public Guid TaskId { get; set; }
    public string Status { get; set; } = "Success";
    public int? StatusCode { get; set; }
    public string? ErrorMessage { get; set; }
    public AccessibilityAnalysisResult? Result { get; set; }
}