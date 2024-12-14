using WriteLens.Shared.Models;

namespace WriteLens.Core.Interfaces.Repositories;

public interface IDocumentContentRepositoryInserter
{
    public Task InsertSingleAsync(DocumentContent document);
}