using Futurum.WebApiEndpoint.Micro.Core.Extensions.Validation;

namespace Futurum.WebApiEndpoint.Micro.Core.Extensions;

public static partial class WebApiResultsExtensions
{
    public static Option<ValidationProblem> ToValidationProblem(IResultError resultError, HttpContext context)
    {
        if (resultError is ResultErrorValidation resultErrorValidation)
        {
            var errors = resultErrorValidation.ValidationErrors.ToDictionary(x => x.PropertyName, x => x.ErrorMessages.ToArray());

            var validationProblem = TypedResults.ValidationProblem(errors, instance: context.Request.Path);

            return validationProblem.ToOption();
        }

        return Option.None<ValidationProblem>();
    }
}