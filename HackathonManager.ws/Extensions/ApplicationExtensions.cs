using HackathonManager.ws.Application.Constants;
using HackathonManager.ws.Application.Evaluations.Services;
using HackathonManager.ws.Application.Hackathons.Serives;
using HackathonManager.ws.Application.Submissions.Services;
using HackathonManager.ws.Application.Teams.Services;
using HackathonManager.ws.Application.User.Services;
using HackathonManager.ws.Domain.Entities;
using HackathonManager.ws.Domain.IRepositories;
using HackathonManager.ws.Infrastructure.Data;
using HackathonManager.ws.Infrastructure.Repositories;
using HackathonManager.ws.Middlewares;
using HackathonManager.ws.Options;
using HackathonManager.ws.Options.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;


namespace HackathonManager.ws.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IHackathonService, HackathonService>();
        services.AddScoped<ITeamService, TeamService>();
        services.AddScoped<ISubmissionService, SubmissionService>();
        services.AddScoped<IEvaluationService, EvaluationService>();

        return services;
    }

    public static IServiceCollection ConfigureJWTOptions(this IServiceCollection Services)
    {
        Services.ConfigureOptions<JwtOptionsSetup>();
        return Services;
    }

    public static IServiceCollection AddSqlDefaultServer(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString)
        );

        return services;
    }

    public static IServiceCollection AddApplicationIdentity(this IServiceCollection services)
    {
        services.AddIdentityCore<AppUser>()
            .AddRoles<IdentityRole<int>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    public static IApplicationBuilder UseAppMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<ProblemDetailsExceptionMiddleware>();
        return app;
    }

    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        // apply migration
        using IServiceScope scope = app.Services.CreateScope();
        AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();


        // Seed admin user
        UserManager<AppUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        AppUser? user = await userManager.FindByNameAsync(RoleNames.Admin);

        if (user == null)
        {
            user = new AppUser
            {
                UserName = RoleNames.Admin,
                DisplayName = RoleNames.Admin,
            };

            await userManager.CreateAsync(user, "Password@123");
            await userManager.AddToRoleAsync(user, RoleNames.Admin);
        }
    }

    public static IServiceCollection AddAppliactionCors(this IServiceCollection services)
    {
        services.AddCors(options =>
            options.AddPolicy("AllowAll", policy =>
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()));

        return services;
    }

    public static IApplicationBuilder UseApplicationCors(this IApplicationBuilder app)
    {
        app.UseCors("AllowAll");
        return app;
    }

    public static IServiceCollection AddSwaggerWithAuthentication(this IServiceCollection Services)
    {
        Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
            });

            options.OperationFilter<SecurityRequirementsOperationFilter>();
        });

        using IServiceScope scope = Services.BuildServiceProvider().CreateScope();
        JwtOptions jwtOptions = scope.ServiceProvider.GetRequiredService<IOptions<JwtOptions>>().Value;
        bool isProd = scope.ServiceProvider.GetRequiredService<IHostEnvironment>().IsProduction();

        Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options => options.TokenValidationParameters = new()
        {
            ValidateLifetime = true,
            ValidateIssuer = isProd,
            ValidateAudience = isProd,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
        });
        return Services;
    }
}
