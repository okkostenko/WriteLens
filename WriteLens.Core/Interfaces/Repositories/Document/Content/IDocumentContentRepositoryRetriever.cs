using WriteLens.Shared.Models;
using WriteLens.Core.Application.Commands.Document;
using WriteLens.Core.Models.DomainModels.Document;

namespace WriteLens.Core.Interfaces.Repositories;

public interface IDocumentContentRepositoryRetriever
{
    public Task<DocumentContentWithAnalysisData?> GetSingleByIdAsync(Guid documentId);
    public Task<List<DocumentContentWithAnalysisData>> GetManyByDocumentsIdsAsync(List<Guid> documentsIds, DocumentQueryParams? queryParams);
}