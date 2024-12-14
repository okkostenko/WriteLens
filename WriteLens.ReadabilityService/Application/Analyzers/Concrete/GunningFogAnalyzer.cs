using System.Text.RegularExpressions;
using WriteLens.Shared.Models;
using WriteLens.Shared.Utilities;
using WriteLens.Shared.Constants;
using WriteLens.Readability.Interfaces;
using WriteLens.Readability.Models.ApplicationModels;
using WriteLens.Readability.Application.Analyzers.Interfaces;

namespace WriteLens.Readability.Application.Analyzers;

public class GunningFogAnalazer : IAnalyzer
{
    private readonly DocumentTypeRuleset _ruleset;
    private readonly DaleChalWordList _daleChalWordList;

    public GunningFogAnalazer(DocumentTypeRuleset ruleset, DaleChalWordList daleChalWordList)
    {
        _ruleset = ruleset;
        _daleChalWordList = daleChalWordList;
    }

    public async Task<decimal> AnalyzeAsync(TextProperties text)
    {
        if (text.CharCount == 0)
            return 100;
            
        var ComplexWordsCount = GetComplexWordCount(text.Text); 
        
        decimal index = 206.835m - 1.015m * (text.WordsCount / text.SentancesCount) - 84.6m * (ComplexWordsCount / text.WordsCount);
        decimal score = ConvertIndexToScore(index);
        
        if (score <= (decimal)_ruleset.MaxComplexityScore)
            return score;

        var penalty = (score - (decimal)_ruleset.MaxComplexityScore) / (decimal)_ruleset.MaxComplexityScore * (decimal)_ruleset.ComplexityPenalty;
        return await Task.FromResult(Math.Max(0, 100 - penalty));
    }

    private int GetComplexWordCount(string text)
    {
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(word => word.ToLower().Trim('.', ',', ';', ':', '?', '!'));

        int complexWordsCount = 0;
        foreach (string word in words)
        {
            if (! _daleChalWordList.CommonWords.Contains(word))
            complexWordsCount ++;
        }
        return complexWordsCount;
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
