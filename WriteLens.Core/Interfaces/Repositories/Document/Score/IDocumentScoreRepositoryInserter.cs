using WriteLens.Shared.Models;

namespace WriteLens.Core.Interfaces.Repositories;

public interface IDocumentScoreRepositoryInserter
{
    public Task InsertSingleAsync(DocumentContentScore score);
}