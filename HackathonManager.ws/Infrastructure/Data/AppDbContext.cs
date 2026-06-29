using HackathonManager.ws.Application.Constants;
using HackathonManager.ws.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HackathonManager.ws.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser, IdentityRole<int>, int>(options)
{
    public DbSet<Hackathon> Hackathons => Set<Hackathon>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<Submission> Submissions => Set<Submission>();
    public DbSet<Evaluation> Evaluations => Set<Evaluation>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Seed roles
        builder.Entity<IdentityRole<int>>().HasData(
            new IdentityRole<int>
            {
                Id = 1,
                Name = RoleNames.Admin,
                NormalizedName = RoleNames.Admin.ToUpper(),
            },
            new IdentityRole<int>
            {
                Id = 2,
                Name = RoleNames.Mentor,
                NormalizedName = RoleNames.Mentor.ToUpper(),
            },
            new IdentityRole<int>
            {
                Id = 3,
                Name = RoleNames.Participant,
                NormalizedName = RoleNames.Participant.ToUpper(),
            }
        );
    }
}
