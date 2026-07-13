using HackathonManager.ws.Application.Constants;
using HackathonManager.ws.Application.Pagination;
using System.ComponentModel.DataAnnotations;

namespace HackathonManager.ws.Application.User.Dtos;

public class FilterUserDto : IPaginationFilter
{
    public string? Name { get; set; }
    public string? Role { get; set; }

    [Range(PaginationValues.DefaultPage, int.MaxValue, ErrorMessage = ValidationMessages.PageValidationMessage)]
    public int Page { get; set; } = PaginationValues.DefaultPage;
    [Range(PaginationValues.MinPageSize, PaginationValues.MaxPageSize, ErrorMessage = ValidationMessages.PageSizeValidationMessage)]
    public int PageSize { get; set; } = PaginationValues.DefaultPageSize;
}
