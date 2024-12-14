using System.ComponentModel.DataAnnotations;

namespace WriteLens.Core.WebAPI.DTOs.Document.Requests;

public class GetDocumentsQueryParamsDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Page must be at least 1.")]
    public int Page { get; set; } = 1;

    [Range(1, 30, ErrorMessage = "Size of the page must be between 1 and 30.")]
    public int Size { get; set; } = 10;

    public string? Search { get; set; }

    public string? SortBy { get; set; }

    [RegularExpression("asc|desc", ErrorMessage = "SortDirection must be 'asc' or 'desc'.")]
    public string SortDirection { get; set; } = "asc";
}