using HackathonManager.ws.Application.Constants;
using HackathonManager.ws.Application.Pagination;
using System.ComponentModel.DataAnnotations;

namespace HackathonManager.ws.Application.Evaluations.Dtos;

public class FilterEvaluationDto : IPaginationFilter
{
    public int? HackathonId { get; set; }
    public int? TeamId { get; set; }
    public int? SubmissionId { get; set; }

    [Range(PaginationValues.DefaultPage, int.MaxValue, ErrorMessage = ValidationMessages.PageValidationMessage)]
    public int Page { get; set; } = PaginationValues.DefaultPage;
    [Range(PaginationValues.MinPageSize, PaginationValues.MaxPageSize, ErrorMessage = ValidationMessages.PageSizeValidationMessage)]
    public int PageSize { get; set; } = PaginationValues.DefaultPageSize;
}
