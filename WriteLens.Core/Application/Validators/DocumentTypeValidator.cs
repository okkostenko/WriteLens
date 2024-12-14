using WriteLens.Core.Exceptions;
using WriteLens.Core.Interfaces.Caching;

namespace WriteLens.Core.Application.Validators;

public static class DocumentTypeValidator
{
    public static async Task ValidateDocumentTypeExists(IDocumentTypeCache documentTypeCache, Guid typeId)
    {
        var documentType = await documentTypeCache.GetDocumentTypeByIdAsync(typeId);
        if (documentType is null)
        {
            throw new DocumentTypeNotFoundException("id", typeId);
        }
    }
}