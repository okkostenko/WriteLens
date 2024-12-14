namespace WriteLens.Core.Interfaces.Repositories;

public interface IDocumentScoreRepositoryUpdater
{
    public Task PullSectionsScoresBySectionsIdsAsync(Guid documentId, List<Guid> sectionsIds);
    public Task PushSectionsScoresAsync(Guid documentId, List<Guid> sectionsIds);
}