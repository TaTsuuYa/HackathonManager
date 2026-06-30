using HackathonManager.ws.Application.Constants;
using HackathonManager.ws.Application.Hackathons.Dtos;
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
}
