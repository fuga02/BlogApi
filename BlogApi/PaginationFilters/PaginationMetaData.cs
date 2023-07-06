namespace BlogApi.PaginationFilters;
public class PaginationMetaData
{
    public PaginationMetaData(int totalCount, int pageSize, int pageNumber, DateTime? fromDate, DateTime? toDate)
    {
        CurrentPage = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        FromDate = fromDate;
        ToDate = toDate;
    }

    public int CurrentPage { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
    public int PageSize { get; init; }
    public DateTime? FromDate { get; init; }
    DateTime? ToDate { get; init; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
}
public record Pagination(int Page, int Count);

public record BlogFilter(
    int Page,
    int Count,
    string? Title,
    DateTime? FromDate,
    DateTime? ToDate) : Pagination(Page, Count);