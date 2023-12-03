using System.Net;

using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Micro.Core.Extensions;

public static partial class WebApiResultsExtensions
{
    public static BadRequest<ProblemDetails> ToWebApiBadRequest(this IResultError error, HttpContext context)
    {
        var problemDetails = error.ToProblemDetails(HttpStatusCode.BadRequest, context);
        
        return TypedResults.BadRequest(problemDetails);
    }
}