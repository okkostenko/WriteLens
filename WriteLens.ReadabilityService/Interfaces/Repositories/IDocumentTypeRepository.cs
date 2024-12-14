using WriteLens.Shared.Models;

namespace WriteLens.Readability.Interfaces.Repositories;

public interface IDocumentTypeRepository
{
    Task<List<DocumentType>> GetManyAsync();
}