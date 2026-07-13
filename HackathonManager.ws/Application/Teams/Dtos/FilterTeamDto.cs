using HackathonManager.ws.Application.Constants;
using HackathonManager.ws.Application.Pagination;
using System.ComponentModel.DataAnnotations;

namespace HackathonManager.ws.Application.Teams.Dtos;

public class FilterTeamDto : IPaginationFilter
{
    public string? Query { get; set; }
    public int? LeaderId { get; set; }
    public int? MemberId { get; set; }

    [Range(PaginationValues.DefaultPage, int.MaxValue, ErrorMessage = ValidationMessages.PageValidationMessage)]
    public int Page { get; set; } = PaginationValues.DefaultPage;
    [Range(PaginationValues.MinPageSize, PaginationValues.MaxPageSize, ErrorMessage = ValidationMessages.PageSizeValidationMessage)]
    public int PageSize { get; set; } = PaginationValues.DefaultPageSize;
}
