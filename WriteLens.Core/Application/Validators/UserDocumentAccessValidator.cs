
using WriteLens.Core.Models.DomainModels.Document;

namespace WriteLens.Core.Application.Validators;

public static class UserDocumentAccessValidator
{
    public static void ValidateUserHasAccessToDocument(Guid userId, Document document)
    {
        if (document.User.Id == userId) return;

        throw new AccessDeniedException(userId, "document", document.Id);
    }
}