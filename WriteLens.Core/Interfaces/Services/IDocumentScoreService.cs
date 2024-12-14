namespace WriteLens.Core.Interfaces.Services;

public interface IDocumentScoreService
{
    public Task CreateSingleAsync(Guid documentId);

    public Task DeleteSectionsScoresBySectionsIds(
        Guid documentId,
        List<Guid>? sectionsIds);
}