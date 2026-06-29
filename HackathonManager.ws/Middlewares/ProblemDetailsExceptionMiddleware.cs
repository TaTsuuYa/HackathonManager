using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;

namespace HackathonManager.ws.Middlewares;

public class ProblemDetailsExceptionMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, ProblemDetailsFactory problemDetailsFactory, IHostEnvironment env)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            ProblemDetails problemDetails = problemDetailsFactory.CreateProblemDetails(
                context,
                statusCode: (int)HttpStatusCode.InternalServerError,
                title: "An unexpected error occurred.",
                detail: env.IsDevelopment() ? ex.Message : "A server error occurred. Please contact support.",
                instance: context.Request.Path
            );

            context.Response.StatusCode = problemDetails.Status ?? 500;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
