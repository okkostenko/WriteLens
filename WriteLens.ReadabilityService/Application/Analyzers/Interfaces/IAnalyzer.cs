using WriteLens.Shared.Models;

namespace WriteLens.Readability.Application.Analyzers.Interfaces;

public interface IAnalyzer
{
    Task<decimal> AnalyzeAsync(TextProperties text);
}