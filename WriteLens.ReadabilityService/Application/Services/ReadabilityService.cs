using AutoMapper;
using WriteLens.Readability.Application.Analyzers;
using WriteLens.Shared.Exceptions.AnalysisExceptions;
using WriteLens.Shared.Exceptions.DocumentExceptions;
using WriteLens.Shared.Interfaces.Caching;
using WriteLens.Readability.Interfaces.Repositories;
using WriteLens.Readability.Interfaces.Services;
using WriteLens.Readability.Models.ApplicationModels;
using WriteLens.Shared.Models;
using WriteLens.Shared.Constants;

namespace WriteLens.Readability.Application.Services;

public class ReadabilityService : IReadabilityService
{
    private readonly IDocumentContentRepository _documentContentRepository;
    private readonly IDocumentScoreRepository _documentScoreRepository;
    private readonly IDocumentTypeCache _documentTypeCache;
    private readonly DaleChalWordList _daleChalWordList;
    public readonly IMapper _mapper;

    public ReadabilityService(
        IDocumentContentRepository documentContentRepository,
        IDocumentScoreRepository documentScoreRepository,
        IDocumentTypeCache documentTypeCache,
        IMapper mapper,
        DaleChalWordList daleChalWordList
        )
    {
        _documentContentRepository = documentContentRepository;
        _documentScoreRepository = documentScoreRepository;
        _documentTypeCache = documentTypeCache;
        _mapper = mapper;
        _daleChalWordList = daleChalWordList;
    }

    public async Task<DocumentContentDocumentScore> AnalyzeAsync(Guid documentId)
    {
        DocumentContent documentContent = await GetDocumentContentAsync(documentId);
        DocumentTypeRuleset analysisRuleset = await RetrieveDocumentTypeRulesetFromCacheAsync(documentContent.TypeId);
        DocumentContentScore documentContentScore = await GetDocumentContentScoreAsync(documentId);

        List<DocumentContentSection> awaitingAnalysis = FilterSectionsAwaitingAnalysis(documentContent.Sections);
        List<TextAnalysisResult> analyzedContentSections = await AnalyzeSections(analysisRuleset, awaitingAnalysis);
        
        DocumentContentLength documentLength = GetDocumentContentLength(documentContent);
        documentContentScore = UpdateDocumentScores(documentLength, documentContentScore, analyzedContentSections);
        
        await _documentScoreRepository.UpdateSingleByDocumentIdAsync(documentId, documentContentScore);
        await MarkSectionsAsAnalyzed(documentId, awaitingAnalysis);

        return documentContentScore.DocumentScore;
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
        var awaitingAnalysis = documentSections.Where(s => s.IsReadabilityAnalyzed == false).ToList();
        if (awaitingAnalysis is null || awaitingAnalysis.Count() == 0)
            throw new NothigToAnalyzeException();
        return awaitingAnalysis;
    }

    private async Task<List<TextAnalysisResult>> AnalyzeSections(DocumentTypeRuleset ruleset, List<DocumentContentSection> sections)
    {
        var orchestrator = new ReadabilityAnalysisOrchestrator(ruleset, _daleChalWordList);

        var tasks = sections.Select(s => orchestrator.AnalyzeAsync(s.Id, s.Content));
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
            ReadabilityScore = 0,
            AccessibilityScore = documentScore.DocumentScore.AccessibilityScore
        };

        foreach (DocumentContentSectionScore score in documentScore.SectionsScores)
        {
            int sectionLength;
            if (updatedSectionsScores.ContainsKey(score.SectionId))
            {
                score.ReadabilityScore = updatedSectionsScores[score.SectionId].Score;
                sectionLength = updatedSectionsScores[score.SectionId].TextLength;
            }
            else
            {
                sectionLength = documentContentLength.SectionsLengthes[score.SectionId];
            }
            score.LastUpdated = DateTimeOffset.UtcNow;

            updatedDocumentScore.ReadabilityScore += score.ReadabilityScore * sectionLength;
            updatedDocumentScore.AccessibilityScore += score.AccessibilityScore != null
                ? score.AccessibilityScore * sectionLength
                : 100;
        }
        updatedDocumentScore.ReadabilityScore /= documentContentLength.DocumentLength;
        updatedDocumentScore.LastUpdated = DateTimeOffset.UtcNow;
        
        documentScore.DocumentScore = updatedDocumentScore;
        return documentScore;
    }

    private async Task MarkSectionsAsAnalyzed(Guid documentId, List<DocumentContentSection> analyzedSections)
    {
        List<Guid> analyzedSectionsIds = analyzedSections.Select(s => s.Id).ToList();
        await _documentContentRepository.MarkDocumentSectionsAsAnalyzedAsync(documentId, analyzedSectionsIds);
    }
}