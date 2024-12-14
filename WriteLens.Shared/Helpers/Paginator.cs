using WriteLens.Shared.Models;

namespace WriteLens.Shared.Helpers;

public static class Paginator<T>
{
    public static PaginatedList<T> Paginate(List<T> items, PaginationParams paginationParams)
    {
        int totalCount = (int)Math.Ceiling((double)items.Count / paginationParams.Size);

        items = items
            .Skip(paginationParams.Skip)
            .Take(paginationParams.Size)
            .ToList();

        return new PaginatedList<T>
        {
            PaginationInfo = new PaginationInfo
            {
                TotalCount = totalCount,
                CurrentPage = paginationParams.Page,
                PageSize = paginationParams.Size
            },
            Items = items
        };
    }
}