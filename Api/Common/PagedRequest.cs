using FastEndpoints;

namespace Api.Common;

public class PagedRequest
{
    [QueryParam]
    [BindFrom("pageSize")]
    public int PageSize { get; set; }
    [QueryParam]
    [BindFrom("page")]
    public int Page { get; set; }
    
}