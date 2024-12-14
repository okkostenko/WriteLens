using WriteLens.Shared.Models;

namespace WriteLens.Accessibility.Interfaces.Repositories;

public interface IDocumentTypeRepository
{
    Task<List<DocumentType>> GetManyAsync();
}