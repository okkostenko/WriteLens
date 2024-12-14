using WriteLens.Core.Application.Commands.Document;
using WriteLens.Core.Models.DomainModels.Document;
using WriteLens.Shared.Models;

namespace WriteLens.Core.Interfaces.Services;

public interface IDocumentServiceDelete
{
    Task DeleteSingleById(Guid userId, Guid documentId);
}