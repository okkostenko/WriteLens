namespace WriteLens.Core.WebAPI.DTOs.Pagination;

public class PaginationInfoResponseDto
{
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
}