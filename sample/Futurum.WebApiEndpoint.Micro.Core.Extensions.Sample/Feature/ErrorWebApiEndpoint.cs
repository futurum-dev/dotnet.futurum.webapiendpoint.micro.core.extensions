namespace Futurum.WebApiEndpoint.Micro.Core.Extensions.Sample.Feature;

[WebApiEndpoint(prefixRoute: "error", group: "feature")]
public partial class ErrorWebApiEndpoint
{
    protected override void Build(IEndpointRouteBuilder builder)
    {
        builder.MapGet("exception", ExceptionHandler);

        builder.MapGet("result-error", ResultErrorHandler);

        builder.MapGet("exception-with-result-error", ExceptionWithResultErrorHandler);
    }

    private static Ok ExceptionHandler()
    {
        throw new Exception("We have an Exception!");

        return TypedResults.Ok();
    }

    private static Results<Ok, BadRequest<ProblemDetails>> ResultErrorHandler(HttpContext context) =>
        Result.Fail("We have a ResultError!")
              .ToWebApi(context, ToOk);

    private static Results<Ok, BadRequest<ProblemDetails>> ExceptionWithResultErrorHandler(HttpContext context) =>
        Result.Try(() => throw new Exception("We have an Exception!"), () => "Exception to ResultError")
              .ToWebApi(context, ToOk);
}
