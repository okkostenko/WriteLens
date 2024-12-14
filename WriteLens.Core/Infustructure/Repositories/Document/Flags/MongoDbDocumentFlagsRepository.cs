using WriteLens.Shared.Models;
using WriteLens.Core.Interfaces.Repositories;
using WriteLens.Core.Application.Commands.Document;
using WriteLens.Core.Models.DomainModels.Document;

namespace WriteLens.Core.Infrastructure.Repositories;

public class MongoDbDocumentFlagsRepository : IDocumentFlagsRepository
{
    private readonly IDocumentFlagsRepositoryDeleter _deleter;

    public MongoDbDocumentFlagsRepository (
        IDocumentFlagsRepositoryDeleter deleter
    )
    {
        _deleter = deleter;
    }

    public async Task DeleteManyByDocumentIdAsync(Guid documentId)
    {
        await _deleter.DeleteManyByDocumentIdAsync(documentId);
    }
    public async Task DeleteManyBySectionIdAsync(Guid documentId, Guid sectionId)
    {
        await _deleter.DeleteManyBySectionIdAsync(documentId, sectionId);
    }

    public async Task DeleteManyBySectionIdAsync(Guid documentId, List<Guid> sectionsId)
    {
        await _deleter.DeleteManyBySectionIdAsync(documentId, sectionsId);
    }
}