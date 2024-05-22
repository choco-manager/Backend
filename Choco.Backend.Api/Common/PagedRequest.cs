using FastEndpoints;

namespace Choco.Backend.Api.Common;

public class PagedRequest
{
    [QueryParam]
    [BindFrom("pageSize")]
    public int PageSize { get; set; }
    [QueryParam]
    [BindFrom("page")]
    public int Page { get; set; }

    public int Skip => (Page - 1) * PageSize;

}