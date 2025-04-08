using X.PagedList;

namespace App.Helper.Dto
{ }
public class PaginatedResult<T>
{
    public IPagedList<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    public string searchString { get; set; }

    public UserDto UserDto { get; set; }
}

