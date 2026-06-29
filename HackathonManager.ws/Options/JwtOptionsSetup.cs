using HackathonManager.ws.Options.Models;
using Microsoft.Extensions.Options;

namespace HackathonManager.ws.Options;

public class JwtOptionsSetup(IConfiguration Configuration) : IConfigureOptions<JwtOptions>
{
    private const string SectionName = "Jwt";
    private readonly IConfiguration _configuration = Configuration;

    public void Configure(JwtOptions options)
        => _configuration
            .GetSection(SectionName)
            .Bind(options);
}

