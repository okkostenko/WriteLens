using WriteLens.Readability.Application.Analyzers.Interfaces;
using WriteLens.Shared.Models;

namespace WriteLens.Readability.Application.Analyzers;

public class ARIAnalyzer : IAnalyzer
{
    private readonly DocumentTypeRuleset _ruleset;

    public ARIAnalyzer(DocumentTypeRuleset ruleset)
    {
        _ruleset = ruleset;
    }

    public async Task<decimal> AnalyzeAsync(TextProperties text)
    {
        if (text.CharCount == 0)
            return 100;
        
        var index = 4.71m * (text.CharCount / text.WordsCount) + 0.5m * (text.WordsCount / text.SentancesCount) - 21.43m;
        var score = ConvertIndexToScore(index);

        var penalty = (score - (decimal)_ruleset.MaxComplexityScore) / (decimal)_ruleset.MaxComplexityScore * (decimal)_ruleset.ComplexityPenalty;
        return await Task.FromResult(Math.Max(0, 100 - penalty));
    }
    
    private decimal ConvertIndexToScore(decimal index)
    {
        if (index <= 6)
            return 100m;
        else if (index <= 7)
            return 90m;
        else if (index <= 9)
            return 70m;
        else if (index <= 12)
            return 60m;
        else if (index <= 16)
            return 40m;
        else
            return 20m;
    }
}