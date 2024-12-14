using WriteLens.Core.Application.Commands.Document;
using WriteLens.Core.Models.DomainModels.Document;
using WriteLens.Shared.Models;

namespace WriteLens.Core.Interfaces.Services;

public interface IDocumentServiceRead
{
    Task<Document> GetSingleByIdAsync(Guid userId, Guid documentId);
    Task<PaginatedList<Document>>  GetManyByUserIdAsync(Guid userId, DocumentQueryParams? queryParams);
}