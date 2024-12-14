namespace WriteLens.Core.Interfaces.Repositories;

public interface IDocumentContentRepositoryDeleter
{
    public Task DeleteSingleByIdAsync(Guid documentId);
}