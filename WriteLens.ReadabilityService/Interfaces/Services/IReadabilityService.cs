using WriteLens.Shared.Models;

namespace WriteLens.Readability.Interfaces.Services;

public interface IReadabilityService
{
    public Task<DocumentContentDocumentScore> AnalyzeAsync(Guid documentId);
}