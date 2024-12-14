using WriteLens.Readability.Application.Analyzers.Interfaces;
using WriteLens.Shared.Models;

namespace WriteLens.Readability.Application.Analyzers;

public class FleschKincaidAnalyzer : IAnalyzer
{
    private readonly DocumentTypeRuleset _ruleset;

    public FleschKincaidAnalyzer(DocumentTypeRuleset ruleset)
    {
        _ruleset = ruleset;
    }

    public async Task<decimal> AnalyzeAsync(TextProperties text)
    {
        if (text.CharCount == 0)
            return 100;
        
        var score = (
            0.39m * (text.WordsCount / text.SentancesCount) +
            11.8m * (text.SyllablesCount / text.WordsCount) -
            15.59m);

        var penalty = (
            (score - (decimal)_ruleset.MaxComplexityScore) /
            (decimal)_ruleset.MaxComplexityScore *
            (decimal)_ruleset.ComplexityPenalty);
        
        return await Task.FromResult(Math.Max(0, 100 - penalty));
    } 
}