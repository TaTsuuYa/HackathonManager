using HackathonManager.ws.Application.Constants;
using System.ComponentModel.DataAnnotations;

namespace HackathonManager.ws.Application.Pagination;

public interface IPaginationFilter
{
    [Range(PaginationValues.DefaultPage, int.MaxValue, ErrorMessage = ValidationMessages.PageValidationMessage)]
    int Page { get; set; }
    [Range(PaginationValues.MinPageSize, PaginationValues.MaxPageSize, ErrorMessage = ValidationMessages.PageSizeValidationMessage)]
    int PageSize { get; set; }
}
