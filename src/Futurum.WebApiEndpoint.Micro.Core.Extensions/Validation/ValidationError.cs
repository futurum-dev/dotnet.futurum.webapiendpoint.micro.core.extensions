namespace Futurum.WebApiEndpoint.Micro.Core.Extensions.Validation;

public record ValidationError(string PropertyName, IEnumerable<string> ErrorMessages);