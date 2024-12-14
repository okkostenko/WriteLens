using WriteLens.Core.Interfaces.Repositories;
using WriteLens.Shared.Models;

namespace WriteLens.Core.Interfaces.Services;

public class DocumentScoreService : IDocumentScoreService
{
    private readonly IDocumentScoreRepository _documentScoreRepository;

    public DocumentScoreService (
        IDocumentScoreRepository documentScoreRepository)
    {
        _documentScoreRepository = documentScoreRepository;
    }

    public async Task CreateSingleAsync(Guid documentId)
    {
        var score = new DocumentContentScore
        {
            Id = Guid.NewGuid(),
            DocumentId = documentId,
            DocumentScore = new DocumentContentDocumentScore
                { LastUpdated = DateTimeOffset.Now },
            SectionsScores = new List<DocumentContentSectionScore>()
        };

        await _documentScoreRepository.InsertSingleAsync(score);
    }

    public async Task DeleteSectionsScoresBySectionsIds(
        Guid documentId,
        List<Guid>? sectionsIds)
    {
        if (sectionsIds is null || sectionsIds.Count == 0)
            return;

        await _documentScoreRepository.PullSectionsScoresBySectionsIdsAsync(
            documentId, sectionsIds);
    }
}