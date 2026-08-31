namespace OrchardCore.Cms.KtuSaApi.Endpoints.Shared;

public class PagedResponse<T>
{
    [Description("Items belonging to the requested page.")]
    public required List<T> Items { get; set; }

    [Description("1-based index of the returned page.")]
    public int Page { get; set; }

    [Description("Number of items per page used to build this response.")]
    public int PageSize { get; set; }

    [Description("Total number of items matching the request across every page.")]
    public int TotalCount { get; set; }

    [Description("Total number of pages available for the current page size.")]
    public int TotalPages { get; set; }

    [Description("True when a page before the returned one exists.")]
    public bool HasPreviousPage { get; set; }

    [Description("True when a page after the returned one exists.")]
    public bool HasNextPage { get; set; }
}

public static class PagedResponse
{
    public const int MaxPageSize = 100;

    public static PagedResponse<T> Create<T>(
        IReadOnlyCollection<T> orderedItems,
        int? page,
        int? pageSize,
        int defaultPageSize)
    {
        var size = Math.Clamp(pageSize ?? defaultPageSize, 1, MaxPageSize);
        var totalCount = orderedItems.Count;
        var totalPages = (int)Math.Ceiling(totalCount / (double)size);
        var currentPage = Math.Clamp(page ?? 1, 1, Math.Max(totalPages, 1));

        return new PagedResponse<T>
        {
            Items = [.. orderedItems.Skip((currentPage - 1) * size).Take(size)],
            Page = currentPage,
            PageSize = size,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasPreviousPage = currentPage > 1,
            HasNextPage = currentPage < totalPages
        };
    }
}
