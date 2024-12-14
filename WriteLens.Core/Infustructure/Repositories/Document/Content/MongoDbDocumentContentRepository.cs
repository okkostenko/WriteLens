using WriteLens.Shared.Models;
using WriteLens.Core.Interfaces.Repositories;
using WriteLens.Core.Application.Commands.Document;
using WriteLens.Core.Models.DomainModels.Document;

namespace WriteLens.Core.Infrastructure.Repositories;

public class MongoDbDocumentContentRepository : IDocumentContentRepository
{
    private readonly IDocumentContentRepositoryInserter _inserter;
    private readonly IDocumentContentRepositoryDeleter _deleter;
    private readonly IDocumentContentRepositoryUpdater _updater;
    private readonly IDocumentContentRepositoryRetriever _retriever;

    public MongoDbDocumentContentRepository (
        IDocumentContentRepositoryInserter inserter,
        IDocumentContentRepositoryDeleter deleter,
        IDocumentContentRepositoryUpdater updater,
        IDocumentContentRepositoryRetriever retriever
    )
    {
        _inserter = inserter;
        _deleter = deleter;
        _updater = updater;
        _retriever = retriever;
    }

    public async Task InsertSingleAsync(DocumentContent document)
    {
        await _inserter.InsertSingleAsync(document);
    }

    public async Task DeleteSingleByIdAsync(Guid documentId)
    {
        await _deleter.DeleteSingleByIdAsync(documentId);
    }

    public async Task<List<DocumentContentWithAnalysisData>> GetManyByDocumentsIdsAsync(List<Guid> documentsIds, DocumentQueryParams? queryParams)
    {
        return await _retriever.GetManyByDocumentsIdsAsync(documentsIds, queryParams);
    }

    public async Task<DocumentContentWithAnalysisData?> GetSingleByIdAsync(Guid documentId)
    {     
        return await _retriever.GetSingleByIdAsync(documentId);
    }

    public async Task UpdateSingleByIdAsync(Guid documentId, UpdateDocumentCommand updateDocumentCommand)
    {
        await _updater.UpdateSingleByIdAsync(documentId, updateDocumentCommand);
    }
}