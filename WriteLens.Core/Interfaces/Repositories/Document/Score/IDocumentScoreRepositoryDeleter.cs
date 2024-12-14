using WriteLens.Shared.Models;

namespace WriteLens.Core.Interfaces.Repositories;

public interface IDocumentScoreRepositoryDeleter
{
    public Task DeleteSingleByDocumentIdAsync(Guid documentId);
}