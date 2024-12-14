namespace WriteLens.Core.WebAPI.DTOs.Pagination;

public class PaginatedListResponseDto<T>
{
    public PaginationInfoResponseDto PaginationInfo { get; set; }
    public List<T> Items { get; set; }
}