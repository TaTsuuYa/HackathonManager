namespace HackathonManager.ws.Application.Pagination;

public class PaginatedDto<T> where T : class
{
    public required int Page { get; set; }
    public required int Total { get; set; }
    public required int ItemsPerPage { get; set; }
    public required int TotalPages { get; set; }
    public required IEnumerable<T> Items { get; set; }
}
