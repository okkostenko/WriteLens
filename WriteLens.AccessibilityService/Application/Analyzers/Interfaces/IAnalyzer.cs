using WriteLens.Accessibility.Models.ApplicationModels;

namespace WriteLens.Accessibility.Application.Analyzers.Interfaces;

public interface IAnalyzer
{
    public Task<TextAnalysisResult> AnalyzeAsync(Guid taskId, string text);
}