using AutoMapper;
using WriteLens.Accessibility.Application.Analyzers;
using WriteLens.Shared.Exceptions.AnalysisExceptions;
using WriteLens.Shared.Exceptions.DocumentExceptions;
using WriteLens.Shared.Interfaces.Caching;
using WriteLens.Accessibility.Interfaces.Repositories;
using WriteLens.Accessibility.Interfaces.Services;
using WriteLens.Accessibility.Models.ApplicationModels;
using WriteLens.Shared.Models;
using WriteLens.Accessibility.Settings;
using Microsoft.Extensions.Options;
namespace WriteLens.Accessibility.Application.Services;

public class AccessibilityService : IAccessibilityService
{
    private readonly IDocumentContentRepository _documentContentRepository;
    private readonly IDocumentScoreRepository _documentScoreRepository;
    private readonly IDocumentFlagsRepository _documentFlagsRepository;
    private readonly IDocumentTypeCache _documentTypeCache;
    private readonly IHttpContextAccessor _context;
    private readonly MLTASSettings _mltasSettings;
    private readonly ILogger<AccessibilityService> _logger;
    private readonly ILoggerFactory _loggerFactory;

    public AccessibilityService(
        IDocumentContentRepository documentContentRepository,
        IDocumentScoreRepository documentScoreRepository,
        IDocumentFlagsRepository documentFlagsRepository,
        IDocumentTypeCache documentTypeCache,
        IHttpContextAccessor context,
        IOptions<MLTASSettings> mltasSettings,
        ILogger<AccessibilityService> logger,
        ILoggerFactory loggerFactory)
    {
        _documentContentRepository = documentContentRepository;
        _documentScoreRepository = documentScoreRepository;
        _documentFlagsRepository = documentFlagsRepository;
        _documentTypeCache = documentTypeCache;
        _context = context;
        _mltasSettings = mltasSettings.Value;
        _logger = logger;
        _loggerFactory = loggerFactory;
    }

    public async Task<AccessibilityAnalysisResult> AnalyzeAsync(Guid documentId)
    {
        _logger.LogInformation($"Started analysis of document '{documentId}'");

        DocumentContent documentContent = await GetDocumentContentAsync(documentId);
        DocumentTypeRuleset analysisRuleset = await RetrieveDocumentTypeRulesetFromCacheAsync(documentContent.TypeId);
        DocumentContentScore documentContentScore = await GetDocumentContentScoreAsync(documentId);

        List<DocumentContentSection> awaitingAnalysis = FilterSectionsAwaitingAnalysis(documentContent.Sections);
        _logger.LogInformation($"Retrieved and filtered data to analyze document '{documentId}'");

        List<TextAnalysisResult> analyzedContentSections = await AnalyzeSections(analysisRuleset, awaitingAnalysis);
        _logger.LogInformation($"Analyzed content sections of document '{documentId}'");

        DocumentContentLength documentLength = GetDocumentContentLength(documentContent);
        documentContentScore = UpdateDocumentScores(documentLength, documentContentScore, analyzedContentSections);
        _logger.LogInformation($"Calculated score of document '{documentId}'");

        List<DocumentContentFlag> newFlags = CreateDocumentFlags(documentId, analyzedContentSections, documentContent.Sections);
        _logger.LogInformation($"Created document '{documentId}' content flags");

        await _documentScoreRepository.UpdateSingleByDocumentIdAsync(documentId, documentContentScore);
        await UpdateDocumentFlags(awaitingAnalysis, newFlags);
        await MarkSectionsAsAnalyzed(documentId, awaitingAnalysis);
        _logger.LogInformation($"Updated document '{documentId}' record in database");

        List<DocumentContentFlag>? documentContentFlags = await _documentFlagsRepository.GetManyByDocumentIdAsync(documentId);
        
        return new AccessibilityAnalysisResult
        {
            Score = documentContentScore.DocumentScore,
            Flags = documentContentFlags
        };
    }

    private async Task<DocumentContent> GetDocumentContentAsync(Guid documentId)
    {
        DocumentContent? documentContent = await _documentContentRepository.GetSingleByIdAsync(documentId);
        if (documentContent is null)
            throw new DocumentNotFoundException("id", documentId);
        if (documentContent.Sections is null)
            throw new NothigToAnalyzeException("id", documentId);
        
        return documentContent;
    }

    private async Task<DocumentTypeRuleset> RetrieveDocumentTypeRulesetFromCacheAsync(Guid documentTypeId)
    {
        var documentType = await _documentTypeCache.GetDocumentTypeByIdAsync(documentTypeId);
        if (documentType is null)
            throw new UnsupportedDocumentTypeException("id", documentTypeId);
        return documentType.Ruleset;
    }

    private List<DocumentContentSection> FilterSectionsAwaitingAnalysis(List<DocumentContentSection> documentSections)
    {
        var awaitingAnalysis = documentSections.Where(s => s.IsAccessibilityAnalyzed == false).ToList();
        if (awaitingAnalysis is null || awaitingAnalysis.Count() == 0)
            throw new NothigToAnalyzeException();
        return awaitingAnalysis;
    }

    private async Task<List<TextAnalysisResult>> AnalyzeSections(DocumentTypeRuleset ruleset, List<DocumentContentSection> sections)
    {
        var analyzer = new AccessibilityAnalyzer(ruleset, _mltasSettings, _loggerFactory);

        var tasks = sections.Select(s => analyzer.AnalyzeAsync(s.Id, s.Content));
        IEnumerable<TextAnalysisResult>? results = await Task.WhenAll(tasks);
        return results.ToList();
    }

    private async Task<DocumentContentScore?> GetDocumentContentScoreAsync(Guid documentId)
    {
        DocumentContentScore? documentContentScore = await _documentScoreRepository.GetSingleByDocumentIdAsync(documentId);
        if (documentContentScore is null)
            throw new Exception($"Something went terribly wrong");
        return documentContentScore;
    }

    private DocumentContentLength GetDocumentContentLength(DocumentContent documentContent)
    {
        int totalLength = 0;
        Dictionary<Guid, int> sectionsLengthes = new Dictionary<Guid, int>();

        foreach (DocumentContentSection section in documentContent.Sections)
        {
            int sectionLength = section.Content.Length;
            totalLength += sectionLength;
            sectionsLengthes[section.Id] = sectionLength;
        }
        return new DocumentContentLength
        {
            DocumentLength = totalLength,
            SectionsLengthes = sectionsLengthes
        };
    }

    private DocumentContentScore UpdateDocumentScores(
        DocumentContentLength documentContentLength,
        DocumentContentScore documentScore,
        List<TextAnalysisResult> analysisResults)
    {
        Dictionary<Guid, TextAnalysisResult> updatedSectionsScores = analysisResults.ToDictionary(s => (Guid)s.Id);

        var updatedDocumentScore = new DocumentContentDocumentScore{
            ReadabilityScore = documentScore.DocumentScore.ReadabilityScore,
            AccessibilityScore = 0
        };

        foreach (DocumentContentSectionScore score in documentScore.SectionsScores)
        {
            int sectionLength;
            if (updatedSectionsScores.ContainsKey(score.SectionId))
            {
                score.AccessibilityScore = updatedSectionsScores[score.SectionId].Score;
                sectionLength = updatedSectionsScores[score.SectionId].TextLength;
            }
            else
            {
                sectionLength = documentContentLength.SectionsLengthes[score.SectionId];
            }
            score.LastUpdated = DateTimeOffset.UtcNow;

            updatedDocumentScore.AccessibilityScore += score.AccessibilityScore * sectionLength;
        }
        updatedDocumentScore.AccessibilityScore /= documentContentLength.DocumentLength;
        updatedDocumentScore.LastUpdated = DateTimeOffset.UtcNow;
        
        documentScore.DocumentScore = updatedDocumentScore;
        return documentScore;
    }

    private List<DocumentContentFlag> CreateDocumentFlags(
        Guid documentId,
        List<TextAnalysisResult> analysisResults,
        List<DocumentContentSection> documentSections
    )
    {
        List<DocumentContentFlag> createdFlags = new List<DocumentContentFlag>();
        Dictionary<Guid, DocumentContentSection> documentSectionsDict = documentSections.ToDictionary(s => s.Id);

        foreach (TextAnalysisResult sectionResult in analysisResults)
        {
            var sectionContent = documentSectionsDict[sectionResult.Id].Content;
            foreach (MLTextAnalysisResultFlag flag in sectionResult.Flags)
            {
                createdFlags.Add(new DocumentContentFlag
                {
                    Id = Guid.NewGuid(),
                    DocumentId = documentId,
                    SectionId = sectionResult.Id,
                    Type = flag.FlagType,
                    Severity = FlagSeverityEnumHelper.FromDecimal(flag.Severity),
                    Suggestion = new DocumentContentFlagSuggestion
                    {
                        Text = flag.Suggestion,
                        OldText = flag.OldText,
                        IsApplied = false
                    },
                    Position = new DocumentContentFlagPosition
                    {
                        Start = sectionContent.IndexOf(flag.OldText),
                        End = sectionContent.IndexOf(flag.OldText) + flag.OldText.Length,
                    },
                    CreatedAt = DateTimeOffset.Now
                });
            }
        }
        return createdFlags;
    }

    private async Task UpdateDocumentFlags(List<DocumentContentSection> analyzedSections, List<DocumentContentFlag> newFlags)
    {
        if (newFlags.Count() <= 0)
            return;

        List<Guid> sectionsIds = analyzedSections.Select(s => s.Id).ToList();
        await _documentFlagsRepository.DeleteManyBySectionIdAsync(sectionsIds);
        await _documentFlagsRepository.InserManyAsync(newFlags);
    }

    private async Task MarkSectionsAsAnalyzed(Guid documentId, List<DocumentContentSection> analyzedSections)
    {
        List<Guid> analyzedSectionsIds = analyzedSections.Select(s => s.Id).ToList();
        await _documentContentRepository.MarkDocumentSectionsAsAnalyzedAsync(documentId, analyzedSectionsIds);
    }
}