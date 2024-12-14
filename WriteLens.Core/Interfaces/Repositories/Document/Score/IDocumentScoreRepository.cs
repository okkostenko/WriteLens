namespace WriteLens.Core.Interfaces.Repositories;

public interface IDocumentScoreRepository :
    IDocumentScoreRepositoryInserter,
    IDocumentScoreRepositoryDeleter,
    IDocumentScoreRepositoryUpdater
{
    
}