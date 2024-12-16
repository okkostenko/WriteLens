using WriteLens.Accessibility.Application.Analyzers.Interfaces;
using WriteLens.Accessibility.Models.ApplicationModels;
using WriteLens.Accessibility.Settings;
using WriteLens.Shared.Models;
using WriteLens.Shared.Types;

namespace WriteLens.Accessibility.Application.Analyzers;

public class AccessibilityAnalyzer : IAnalyzer
{
    private readonly DocumentTypeRuleset _ruleset;
    private readonly MLTextAnalysisAnalyzer _analyzer;
    private readonly ILogger<AccessibilityAnalyzer> _logger;

    public AccessibilityAnalyzer(
        DocumentTypeRuleset ruleset,
        MLTASSettings mltasSettings,
        ILoggerFactory loggerFactory)
    {
        _ruleset = ruleset;
        _analyzer = new MLTextAnalysisAnalyzer(mltasSettings, loggerFactory);
        _logger = loggerFactory.CreateLogger<AccessibilityAnalyzer>();
    }

    public async Task<TextAnalysisResult> AnalyzeAsync(Guid taskId, string text)
    {
        _logger.LogInformation($"Accessibility analysis of section '{taskId}' started.");

        MLTextAnalysisResult result = await _analyzer.AnalyzeAsync(text);
        List<MLTextAnalysisResultFlag> flags = await FilterFlags(result);
        _logger.LogInformation($"Determined flags for section '{taskId}'.");

        decimal score = await CalculateScore(flags);
        _logger.LogInformation($"Calculated accessibility score for section '{taskId}'.");

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