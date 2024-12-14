using WriteLens.Core.Exceptions;
using WriteLens.Core.Models.DomainModels.Document;

namespace WriteLens.Core.Application.Validators;

public static class DocumentValidator
{
    public static void ValidateDocumentExists(Guid documentId, Document? document)
    {
        if (document != null) return;

        throw new DocumentNotFoundException("id", documentId);
    }
}