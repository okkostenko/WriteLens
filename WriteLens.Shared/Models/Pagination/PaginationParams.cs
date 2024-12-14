namespace WriteLens.Shared.Models;

public class PaginationParams
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;

    public int Skip
    {
        get => (Page - 1) * Size;
    }
}