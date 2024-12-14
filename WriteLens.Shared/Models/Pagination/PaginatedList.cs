namespace WriteLens.Shared.Models;

public class PaginatedList<T>
{
    public PaginationInfo PaginationInfo { get; set; }
    public List<T>? Items { get; set; }
}