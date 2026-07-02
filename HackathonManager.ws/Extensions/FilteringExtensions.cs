using HackathonManager.ws.Application.Constants;
using HackathonManager.ws.Application.Hackathons.Dtos;
using HackathonManager.ws.Application.Submissions.Dtos;
using HackathonManager.ws.Application.Teams.Dtos;
using HackathonManager.ws.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HackathonManager.ws.Extensions;

public static class FilteringExtensions
{
    public static IQueryable<Hackathon> Filter(this IQueryable<Hackathon> query, FilterHackathonDto filter)
    {
        if (filter.Theme is not null)
            query = query.Where(h => EF.Functions.Like(h.Theme, $"%{filter.Theme}%"));

        if (filter.Status is not null)
        {
            switch (filter.Status)
            {
                case HackathonStatus.Ended:
                    query = query.Where(h => h.EndDate < DateTime.UtcNow);
                    break;
                case HackathonStatus.Ongoing:
                    query = query.Where(h => h.StartDate <= DateTime.UtcNow && h.EndDate > DateTime.UtcNow);
                    break;
                case HackathonStatus.Upcoming:
                    query = query.Where(h => h.StartDate > DateTime.UtcNow);
                    break;
            }
        }

        return query;
    }

    public static IQueryable<Team> Filter(this IQueryable<Team> query, FilterTeamDto filter)
    {
        if (filter.Query is not null)
        {
            query = query.Where(t => EF.Functions.Like(t.Name, $"%{filter.Query}%") ||
                EF.Functions.Like(t.Description, $"%{filter.Query}%"));
        }

        if (filter.MemberId != null)
            query = query.Where(t => t.Members.Any(m => m.Id == filter.MemberId));

        if (filter.LeaderId != null)
            query = query.Where(t => t.LeaderId == filter.LeaderId);

        return query;
    }

    public static IQueryable<Submission> Filter(this IQueryable<Submission> query, FilterSubmissionDto filter)
    {
        if (filter.Query is not null)
        {
            query = query.Where(t => EF.Functions.Like(t.Title, $"%{filter.Query}%") ||
                EF.Functions.Like(t.Description, $"%{filter.Query}%"));
        }

        if (filter.HackathonId != null)
            query = query.Where(t => t.HackathonId == filter.HackathonId);

        if (filter.TeamId != null)
            query = query.Where(t => t.TeamId == filter.TeamId);

        return query;
    }
}
