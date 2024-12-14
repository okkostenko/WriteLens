using WriteLens.Core.Application.Commands.Document;
using WriteLens.Shared.Models;
using WriteLens.Shared.Utilities;

namespace WriteLens.Core.Helpers;

public static class DocumentContentSectionDataSetter
{
    public static UpdateSectionCommand SetData(
        UpdateSectionCommand newData,
        DocumentContentSection? currnetData)
    {
        newData = newData.Id is null
            ? SetValuesToCreateDocumentContentSection(newData)
            : SetValuesToUpdateDocumentContentSection(newData, currnetData);
        return newData;
    }

    private static UpdateSectionCommand SetValuesToCreateDocumentContentSection(
        UpdateSectionCommand newData)
    {
        newData.Id = Guid.NewGuid();
        newData.Hash = HashUtility.GenerateHash(newData.Content);
        newData.IsAccessibilityAnalyzed = false;
        newData.IsAccessibilityAnalyzed = false;
        newData.State = Models.Types.DocumentContentSectionState.Created;
        
        return newData;
    }

    private static UpdateSectionCommand SetValuesToUpdateDocumentContentSection(
        UpdateSectionCommand newData,
        DocumentContentSection currnetData)
    {
        if (CheckIfContentChanged(newData, currnetData))
        {
            newData.Hash = HashUtility.GenerateHash(newData.Content);
            newData.IsAccessibilityAnalyzed = false;
            newData.IsReadabilityAnalyzed = false;
        }
        else
        {
            newData.Hash = currnetData.Hash;
            newData.IsReadabilityAnalyzed = true;
            newData.IsReadabilityAnalyzed = true;
        }

        newData.State = Models.Types.DocumentContentSectionState.Updated;
        
        return newData;
    }

    private static bool CheckIfContentChanged(
        UpdateSectionCommand newData,
        DocumentContentSection currnetData
    )
    {
        return HashUtility.ValidateHash(newData.Content, currnetData.Hash);
    }
}