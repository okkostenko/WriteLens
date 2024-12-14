using WriteLens.Core.Application.Commands.Document;
using WriteLens.Core.Models.DomainModels.Document;
using WriteLens.Shared.Models;

namespace WriteLens.Core.Interfaces.Services;

public interface IDocumentServiceCreate
{
    Task<Document> CreateSingleAsync(Guid userId, CreateDocumentCommand createDocumentCommand);
}