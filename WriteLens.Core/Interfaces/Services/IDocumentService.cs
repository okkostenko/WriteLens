using WriteLens.Core.Application.Commands.Document;
using WriteLens.Core.Models.DomainModels.Document;
using WriteLens.Shared.Models;

namespace WriteLens.Core.Interfaces.Services;

public interface IDocumentService
{
    Task<Document> GetSingleByIdAsync(Guid userId, Guid documentId);
    Task<PaginatedList<Document>>  GetManyByUserIdAsync(Guid userId, DocumentQueryParams? queryParams);
    Task<Document> CreateSingleAsync(Guid userId, CreateDocumentCommand createDocumentCommand);
    Task UpdateSingleByIdAsync(Guid userId, Guid documentId, UpdateDocumentCommand updateDocumentCommand);
    Task DeleteSingleById(Guid userId, Guid documentId);
}