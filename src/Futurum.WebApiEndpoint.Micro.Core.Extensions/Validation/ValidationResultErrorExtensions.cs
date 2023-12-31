namespace Futurum.WebApiEndpoint.Micro.Core.Extensions.Validation;

/// <summary>
/// Extension methods to transform a DataAnnotations Validation <see cref="ValidationError"/> into a <see cref="IResultError"/> 
/// </summary>
public static class ValidationResultErrorExtensions
{
    /// <summary>
    /// Transform a <see cref="ValidationError"/> into a <see cref="IResultError"/>
    /// </summary>
    public static ResultErrorValidation ToResultError(this IEnumerable<ValidationError> validationResults) =>
        new(validationResults);
}