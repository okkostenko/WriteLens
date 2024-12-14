namespace WriteLens.Core.Interfaces.Repositories;

public interface IDocumentFlagsRepositoryDeleter
{
    public Task DeleteManyByDocumentIdAsync(Guid documentId);
    public Task DeleteManyBySectionIdAsync(Guid documentId, Guid sectionId);
    public Task DeleteManyBySectionIdAsync(Guid documentId, List<Guid> sectionsId);
}