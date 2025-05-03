namespace TableTower.Core.Paging;
public sealed class DefaultPager<T> : IPager<T> where T : class, new()
{
    private readonly IEnumerable<T> _items;

    public int PageSize { get; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    public int TotalItems => _items.Count();
    public int CurrentPage { get; private set; }

    public DefaultPager(IEnumerable<T> items, int pageSize = 10)
    {
        _items = items;
        PageSize = pageSize;
        CurrentPage = 1;
    }

    public IEnumerable<T> GetPageData(int pageNumber)
    {
        if (pageNumber < 0 || pageNumber > TotalPages)
        {
            return [];
        }

        CurrentPage = pageNumber;
        return _items.Skip((CurrentPage - 1) * PageSize).Take(PageSize);
    }
}
