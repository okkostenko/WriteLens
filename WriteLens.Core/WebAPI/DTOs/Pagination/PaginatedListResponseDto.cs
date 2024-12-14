namespace WriteLens.Core.WebAPI.DTOs.Pagination;

/// <summary>
/// Paginated list of items.
/// </summary>
/// <typeparam name="T"></typeparam>
public class PaginatedListResponseDto<T>
{
    /// <summary>
    /// Pagination info that include total number of pages,
    /// current page index and it's size.
    /// </summary>
    public PaginationInfoResponseDto PaginationInfo { get; set; }

    /// <summary>
    /// List of items on the provided page.
    /// </summary>
    public List<T> Items { get; set; }
}