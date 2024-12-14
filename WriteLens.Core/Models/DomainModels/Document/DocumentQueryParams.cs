namespace WriteLens.Core.Models.DomainModels.Document;

public class DocumentQueryParams
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string? Search { get; set; }
    public string? SortBy { get; set; }
    public string SortDirection { get; set; } = "asc";
}