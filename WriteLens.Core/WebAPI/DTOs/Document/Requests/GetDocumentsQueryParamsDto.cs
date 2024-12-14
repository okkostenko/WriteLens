using System.ComponentModel.DataAnnotations;

namespace WriteLens.Core.WebAPI.DTOs.Document.Requests;

/// <summary>
/// Request query parameters to get documents.
/// </summary>
public class GetDocumentsQueryParamsDto
{
    /// <summary>
    /// The page the documents should be queried on when paginated.
    /// </summary>
    /// <remarks>
    /// The index of the page starts from 1.
    /// </remarks>
    /// <value>The default value is 1.</value>
    [Range(1, int.MaxValue, ErrorMessage = "Page must be at least 1.")]
    public int Page { get; set; } = 1;

    /// <summary>
    /// The size of the page when documents are paginated.
    /// </summary> 
    /// <remarks>
    /// The size of the page must be between 1 and 30.
    /// </remarks>
    /// <value>The default value is 10.</value>
    [Range(1, 30, ErrorMessage = "Size of the page must be between 1 and 30.")]
    public int Size { get; set; } = 10;

    /// <summary>
    /// The search string.
    /// </summary>
    /// <remarks>
    /// 'search' may be null.
    /// </remarks>
    public string? Search { get; set; }

    /// <summary>
    /// The field to sort documents by.
    /// </summary>
    /// <remarks>
    /// You can sort by the any parameter of the document's metadata.
    /// The 'sortBy' parametr should be in either camelCase of PascalCase.
    /// 
    /// NOTE: If there is no filed matching the parameter, an error will be raised.
    /// </remarks>
    public string? SortBy { get; set; }

    /// <summary>
    /// The sorting dicrections of the documents.
    /// </summary>
    /// <remarks>
    /// The sorting direction should be either 'asc' or 'desc'.
    /// </remarks>
    /// <value>The default value is acs</value>
    [RegularExpression("asc|desc", ErrorMessage = "SortDirection must be 'asc' or 'desc'.")]
    public string SortDirection { get; set; } = "asc";
}