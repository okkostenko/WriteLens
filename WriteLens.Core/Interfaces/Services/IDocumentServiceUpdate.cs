using WriteLens.Core.Application.Commands.Document;
using WriteLens.Core.Models.DomainModels.Document;

namespace WriteLens.Core.Interfaces.Services;

public interface IDocumentServiceUpdate
{
    public Task<List<UpdateSectionCommand>> UpdateSingleByIdAsync(
        Document document,
        UpdateDocumentCommand updateDocumentCommand);
}