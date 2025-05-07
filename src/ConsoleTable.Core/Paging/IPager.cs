namespace ConsoleTable.Core.Paging;
public interface IPager<T> where T : class
{
    int TotalItems { get; } // total 100 rows
    int TotalPages { get; } // equals to => total items / page size
    int PageSize { get; } // 10 rows per page
    int CurrentPage { get; }

    IEnumerable<T> GetPageData(int pageNumber);
}
