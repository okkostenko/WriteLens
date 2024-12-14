namespace WriteLens.Core.Interfaces.Repositories;

public interface IDocumentContentRepository :
    IDocumentContentRepositoryInserter,
    IDocumentContentRepositoryDeleter,
    IDocumentContentRepositoryUpdater,
    IDocumentContentRepositoryRetriever
{
}