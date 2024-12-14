using WriteLens.Shared.Models;

namespace WriteLens.Accessibility.Models.ApplicationModels;

public class AccessibilityAnalysisResult
{
    public DocumentContentDocumentScore Score { get; set; }
    public List<DocumentContentFlag>? Flags { get; set; }
}