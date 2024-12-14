using WriteLens.Accessibility.Models.ApplicationModels;

namespace WriteLens.Accessibility.Interfaces.Services;

public interface IAccessibilityService
{
    public Task<AccessibilityAnalysisResult> AnalyzeAsync(Guid documentId);
}