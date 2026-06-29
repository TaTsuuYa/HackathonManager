using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HackathonManager.ws.Controllers;

public class ApplicationController : ControllerBase
{
    protected int? GetUserId()
    {
        if (HttpContext.User.Identity is not null && HttpContext.User.Identity.IsAuthenticated)
        {
            Claim? userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim is not null && int.TryParse(userIdClaim.Value, out int userId))
                return userId;
        }

        return null;
    }
}
