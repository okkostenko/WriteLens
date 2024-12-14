namespace WriteLens.Core.WebAPI.DTOs.Pagination;

/// <summary>
/// Pagination info
/// </summary>
public class PaginationInfoResponseDto
{
    /// <summary>
    /// The total count of pages.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// The index of the current page.
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// The size of the page.
    /// </summary>
    public int PageSize { get; set; }
}