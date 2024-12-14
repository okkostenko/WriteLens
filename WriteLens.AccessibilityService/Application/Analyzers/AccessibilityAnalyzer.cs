using WriteLens.Accessibility.Application.Analyzers.Interfaces;
using WriteLens.Accessibility.Models.ApplicationModels;
using WriteLens.Accessibility.Settings;
using WriteLens.Shared.Models;
using WriteLens.Shared.Types;

namespace WriteLens.Accessibility.Application.Analyzers;

public class AccessibilityAnalyzer : IAnalyzer
{
    private const int MAX_WAITING_ITERATIONS = 5;
    
    private readonly DocumentTypeRuleset _ruleset;
    private readonly MLTextAnalysisAnalyzer _client;

    public AccessibilityAnalyzer(DocumentTypeRuleset ruleset, IHttpContextAccessor context, MLTASSettings mltasSettings)
    {
        _ruleset = ruleset;
        _client = new MLTextAnalysisAnalyzer(context, mltasSettings);
    }

    public async Task<TextAnalysisResult> AnalyzeAsync(Guid taskId, string text)
    {
        MLTextAnalysisResult result = await _client.AnalyzeAsync(text);
        List<MLTextAnalysisResultFlag> flags = await FilterFlags(result);

        decimal score = await CalculateScore(flags);

        return new TextAnalysisResult
        {
            Id = taskId,
            Score = score,
            Flags = flags,
            TextLength = text.Length
        };
    }

    private async Task<List<MLTextAnalysisResultFlag>> FilterFlags(MLTextAnalysisResult result)
    {
        List<MLTextAnalysisResultFlag> flags = new ();

        foreach (MLTextAnalysisResultFlag flag in result.Flags)
        {
            if (
                (_ruleset.AllowJargon && flag.FlagType == DocumentContentFlagType.jargon) || 
                (_ruleset.AllowPassiveVoice && flag.FlagType == DocumentContentFlagType.passiveVoice)
            )
            {
                continue;
            }

            flags.Add(flag);
        }

        return flags;
    }

    private async Task<decimal> CalculateScore(List<MLTextAnalysisResultFlag> flags)
    {
        if (flags.Count() == 0)
            return 100.0m;

        decimal score = 0;

        foreach (MLTextAnalysisResultFlag flag in flags)
        {
            double penaltyCoef = GetPenaltyCoefByFlagType(flag.FlagType);
            decimal penalty = flag.Severity * (decimal)penaltyCoef;
            score += 1 - penalty;
        }
        score = score / flags.Count;
        return score * 100;
    }

    private double GetPenaltyCoefByFlagType(DocumentContentFlagType flagType)
    {
        if (flagType == DocumentContentFlagType.jargon)
            return _ruleset.JargonPenalty;
        else if (flagType == DocumentContentFlagType.passiveVoice)
            return _ruleset.PassiveVoicePenalty;
        else if (flagType == DocumentContentFlagType.noninclusiveLanguage)
            return _ruleset.NonInclusiveLanguagePenalty;
        else if (flagType == DocumentContentFlagType.sentenceComplexity)
            return _ruleset.SentenceComplexityPenalty;
        else
            return 0.5;
    }
}