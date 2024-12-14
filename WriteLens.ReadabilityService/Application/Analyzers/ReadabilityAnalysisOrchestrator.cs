using WriteLens.Shared.Models;
using WriteLens.Shared.Constants;
using WriteLens.Readability.Interfaces;
using WriteLens.Readability.Models.ApplicationModels;
using WriteLens.Shared.Utilities;
using WriteLens.Readability.Application.Analyzers.Interfaces;

namespace WriteLens.Readability.Application.Analyzers;

public class ReadabilityAnalysisOrchestrator
{
    private readonly DocumentTypeRuleset _ruleset;
    private readonly DaleChalWordList _daleChalWordList;
    private readonly List<IAnalyzer> _analyzers;

    public ReadabilityAnalysisOrchestrator(DocumentTypeRuleset ruleset, DaleChalWordList daleChalWordList)
    {
        _ruleset = ruleset;
        _daleChalWordList = daleChalWordList;
        _analyzers = new List<IAnalyzer>(){
            new ARIAnalyzer(_ruleset),
            new FleschKincaidAnalyzer(_ruleset),
            new GunningFogAnalazer(_ruleset, daleChalWordList),
            new SmogIndexAnalyzer(_ruleset)
        };
    }

    public async Task<TextAnalysisResult> AnalyzeAsync(Guid taskId, string text)
    {
        var textProperties = GetTextProperties(text);
        var tasks = _analyzers.Select(analyzer => analyzer.AnalyzeAsync(textProperties));

        var results = await Task.WhenAll(tasks);
        return new TextAnalysisResult{
            Id = taskId,
            Score = results.Average(),
            TextLength = textProperties.CharCount
        };
    }

    private TextProperties GetTextProperties(string text)
    {
        return new TextProperties
        {
            Text = text,
            SentancesCount = TextUtitlity.GetSentances(text).Length,
            WordsCount = TextUtitlity.GetWordCount(text),
            SyllablesCount = TextUtitlity.GetSyllablesCount(text)
        };
    }
}