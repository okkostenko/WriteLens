using WriteLens.Accessibility.Models.Commands;
using WriteLens.Shared.Models;

namespace WriteLens.Accessibility.Interfaces.Repositories;

public interface IDocumentFlagsRepository
{
    public Task InsertSingleAsync(DocumentContentFlag documentFlag);
    public Task InserManyAsync(List<DocumentContentFlag> documentFlags);
    public Task DeleteSingleByIdAsync(Guid flagId);
    public Task DeleteManyBySectionIdAsync(Guid sectionId);
    public Task DeleteManyBySectionIdAsync(List<Guid> sectionIds);
    public Task<DocumentContentFlag?> GetSingleByIdAsync(Guid flagId);
    public Task<List<DocumentContentFlag>?> GetManyByDocumentIdAsync(Guid documentId);
    public Task<List<DocumentContentFlag>?> GetManyBySectionIdAsync(Guid sectionId);
    public Task UpdateSingleById(Guid flagId, UpdateDocumentContentFlagCommand updateFlagCommand);
}