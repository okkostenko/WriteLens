using WriteLens.Core.Application.Commands.Document;
using WriteLens.Core.Models.DomainModels.Document;

namespace WriteLens.Core.Interfaces.Repositories;

public interface IDocumentRepository
{
    public Task<Document?> GetSingleByIdAsync(Guid documentId);
    public Task<List<Document>?> GetManyByUserIdAsync(Guid userId, DocumentQueryParams? queryParams);
    public Task AddSingleAsync(Guid userId, Document document);
    public Task UpdateSingleByIdAsync(Guid documentId, UpdateDocumentCommand updateDocumentCommand);
    public Task DeleteSingleByIdAsync(Guid documentId);
}