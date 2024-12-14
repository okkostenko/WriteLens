using WriteLens.Readability.Application.Analyzers.Interfaces;
using WriteLens.Readability.Interfaces;
using WriteLens.Readability.Models.ApplicationModels;
using WriteLens.Shared.Models;
using WriteLens.Shared.Utilities;

namespace WriteLens.Readability.Application.Analyzers;

public class SmogIndexAnalyzer : IAnalyzer
{
    private readonly DocumentTypeRuleset _ruleset;

    public SmogIndexAnalyzer(DocumentTypeRuleset ruleset)
    {
        _ruleset = ruleset;
    }

    public async Task<decimal> AnalyzeAsync(TextProperties text)
    {
        if (text.WordsCount <= 30)
            return 100;

        var totalPolysyllables = text.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Where(word => TextUtitlity.GetSyllablesCount(word) >= 3)
            .Count();

        var index = 1.0430m * (decimal)Math.Sqrt(totalPolysyllables * 30 / text.SentancesCount) + 3.1291m;
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