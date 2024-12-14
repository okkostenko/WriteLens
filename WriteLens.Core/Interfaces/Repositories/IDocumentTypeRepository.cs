
using WriteLens.Shared.Models;

namespace WriteLens.Core.Interfaces.Repositories;

public interface IDocumentTypeRepository
{
    Task<List<DocumentType>> GetManyAsync ();
}