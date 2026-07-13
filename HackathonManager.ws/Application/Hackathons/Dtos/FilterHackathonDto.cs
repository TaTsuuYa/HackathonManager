using HackathonManager.ws.Application.Constants;
using HackathonManager.ws.Application.Pagination;
using System.ComponentModel.DataAnnotations;

namespace HackathonManager.ws.Application.Hackathons.Dtos;

public class FilterHackathonDto : IPaginationFilter
{
    public string? Theme { get; set; }
    public HackathonStatus? Status { get; set; }

    [Range(PaginationValues.DefaultPage, int.MaxValue, ErrorMessage = ValidationMessages.PageValidationMessage)]
    public int Page { get; set; } = PaginationValues.DefaultPage;
    [Range(PaginationValues.MinPageSize, PaginationValues.MaxPageSize, ErrorMessage = ValidationMessages.PageSizeValidationMessage)]
    public int PageSize { get; set; } = PaginationValues.DefaultPageSize;
}
