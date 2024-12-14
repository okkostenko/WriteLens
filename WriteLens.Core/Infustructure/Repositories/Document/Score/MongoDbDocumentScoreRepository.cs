using WriteLens.Shared.Models;
using WriteLens.Core.Interfaces.Repositories;
using WriteLens.Core.Application.Commands.Document;
using WriteLens.Core.Models.DomainModels.Document;

namespace WriteLens.Core.Infrastructure.Repositories;

public class MongoDbDocumentScoreRepository : IDocumentScoreRepository
{
    private readonly IDocumentScoreRepositoryInserter _inserter;
    private readonly IDocumentScoreRepositoryDeleter _deleter;
    private readonly IDocumentScoreRepositoryUpdater _updater;

    public MongoDbDocumentScoreRepository (
        IDocumentScoreRepositoryInserter inserter,
        IDocumentScoreRepositoryDeleter deleter,
        IDocumentScoreRepositoryUpdater updater
    )
    {
        _inserter = inserter;
        _deleter = deleter;
        _updater = updater;
    }

    public async Task InsertSingleAsync(DocumentContentScore score)
    {
        await _inserter.InsertSingleAsync(score);
    }

    public async Task DeleteSingleByDocumentIdAsync(Guid documentId)
    {
        await _deleter.DeleteSingleByDocumentIdAsync(documentId);
    }

    public async Task PullSectionsScoresBySectionsIdsAsync(Guid documentId, List<Guid> sectionsIds)
    {
        await _updater.PullSectionsScoresBySectionsIdsAsync(documentId, sectionsIds);
    }

    public async Task PushSectionsScoresAsync(Guid documentId, List<Guid> sectionsIds)
    {
        await _updater.PushSectionsScoresAsync(documentId, sectionsIds);
    }
}