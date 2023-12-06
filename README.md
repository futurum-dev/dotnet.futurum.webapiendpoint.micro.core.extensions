# Futurum.WebApiEndpoint.Micro.Core.Extensions

![license](https://img.shields.io/github/license/futurum-dev/dotnet.futurum.webapiendpoint.micro.core.extensions?style=for-the-badge)
![CI](https://img.shields.io/github/actions/workflow/status/futurum-dev/dotnet.futurum.webapiendpoint.micro.core.extensions/ci.yml?branch=main&style=for-the-badge)
[![Coverage Status](https://img.shields.io/coveralls/github/futurum-dev/dotnet.futurum.webapiendpoint.micro.core.extensions?style=for-the-badge)](https://coveralls.io/github/futurum-dev/dotnet.futurum.webapiendpoint.micro.core.extensions?branch=main)
[![NuGet version](https://img.shields.io/nuget/v/futurum.webapiendpoint.micro.core.extensions?style=for-the-badge)](https://www.nuget.org/packages/futurum.webapiendpoint.micro.core.extensions)

A dotnet library that extends [Futurum.WebApiEndpoint.Micro](https://www.nuget.org/packages/Futurum.WebApiEndpoint.Micro), making it fully compatible with [Futurum.Core](https://www.nuget.org/packages/Futurum.Core).

- [x] [Full compatibility](#full-compatibility-with-futurumcore) with [Futurum.Core](https://www.nuget.org/packages/Futurum.Core)
- [x] [Built in Validation support](#validation)
  - [x] [Integrated FluentValidation](#fluentvalidationservice)
  - [x] [Integrated DataAnnotations](#dataannotationsvalidationservice)

## Full compatibility with [Futurum.Core](https://www.nuget.org/packages/Futurum.Core)
Comprehensive set of extension methods to transform a [Result](https://docs.futurum.dev/dotnet.futurum.core/result/overview.html) and [Result&lt;T&gt;](https://docs.futurum.dev/dotnet.futurum.core/result/overview.html) to an *TypedResult*.

- If the method passed in is a *success*, then the *IResult* will be returned.
- If the method passed in is a *failure*, then a *BadRequest&lt;ProblemDetails&gt;* will be returned, with the appropriate details set on the ProblemDetails. The *error message* will be safe to return to the client, that is, it will not contain any sensitive information e.g. StackTrace.

The returned type from *ToWebApi* is always augmented to additionally include *BadRequest&lt;ProblemDetails&gt;*

```csharp
Result<T> -> Results<T, BadRequest<ProblemDetails>>

Result<Results<TIResult1, TIResult2>> -> Results<TIResult1, TIResult2, BadRequest<ProblemDetails>>

Result<Results<TIResult1, TIResult2, TIResult3>> -> Results<TIResult1, TIResult2, TIResult3, BadRequest<ProblemDetails>>

Result<Results<TIResult1, TIResult2, TIResult3, TIResult4>> -> Results<TIResult1, TIResult2, TIResult3, TIResult4, BadRequest<ProblemDetails>>

Result<Results<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5>> -> Results<TIResult1, TIResult2, TIResult3, TIResult5, BadRequest<ProblemDetails>>
```

*Results* has a maximum of 6 types. So 5 are allowed leaving one space left for the *BadRequest&lt;ProblemDetails&gt;*.

#### How to handle *successful* and *failure* cases in a typed way with *TypedResult*
You can optionally specify which TypedResult success cases you want to handle. This is useful if you want to handle a specific successes case differently.

You can specify which TypedResult error cases you want to handle. This is useful if you want to handle a specific error case differently.

If you have a *success* case, you must pass in the the *success* helper function first, then the *failure* helper functions.

There can only be 1 *success* helper function, but there can be multiple *failure* helper functions.

##### Example use
The *ToWebApi* extension method will change the method return type to add *BadRequest&lt;ProblemDetails&gt;*, with the appropriate details set on the ProblemDetails. The *error message* will be safe to return to the client, that is, it will not contain any sensitive information e.g. StackTrace.

You can then pass in additional helper functions to deal with successes and failures and these will change the return type to the appropriate *TypedResult*'s.

*ToOk* is a function that will convert a *T* to an *Ok&lt;T&gt;*.

*ToValidationProblem* is a function that will convert a *ValidationResultError* to a *ValidationProblem*.

#### Full Example
```csharp
private static Results<Ok<ArticleDto>, ValidationProblem, BadRequest<ProblemDetails>> ValidationHandler(HttpContext context, IValidationService<ArticleDto> validationService,
                                                                                                        ArticleDto articleDto) =>
    validationService.Execute(articleDto)
                     .Map(() => new Article(null, articleDto.Url))
                     .Map(ArticleMapper.MapToDto)
                     .ToWebApi(context, ToOk, ToValidationProblem);
```

## Success and Failure helper functions
If you have a *success* case, you must pass in the the *success* helper function first, then the *failure* helper functions.

There can only be 1 *success* helper function, but there can be multiple *failure* helper functions.

**Note:** It is recommended to add the following to your *GlobalUsings.cs* file.
```csharp
global using static Futurum.WebApiEndpoint.Micro.WebApiResultsExtensions;
```

This means you can use the helper functions without having to specify the namespace. As in the examples.

### Failure helper functions
#### ToNotFound
If a *ResultErrorKeyNotFound* has occured then it will convert it to a *NotFound&lt;ProblemDetails&gt;*, with the correct information set on the *ProblemDetails*.

```csharp
ToNotFound
```

#### ToValidationProblem
If a *ResultErrorValidation* has occured then it will convert it to a *ValidationProblem*, with the correct information set on the *HttpValidationProblemDetails*.

```csharp
ToValidationProblem
```

## Validation
### ValidationService
Executes FluentValidation and DataAnnotations
```csharp
IValidationService<ArticleDto> validationService
```

e.g.
```csharp
private static Results<Ok<ArticleDto>, ValidationProblem, BadRequest<ProblemDetails>> ValidationHandler(HttpContext context, IValidationService<ArticleDto> validationService,
                                                                                                        ArticleDto articleDto) =>
    validationService.Execute(articleDto)
                     .Map(() => new Article(null, articleDto.Url))
                     .Map(ArticleMapper.MapToDto)
                     .ToWebApi(context, ToOk, ToValidationProblem);
```

### FluentValidationService
Calls FluentValidation
```csharp
IFluentValidationService<ArticleDto> fluentValidationService
```

e.g.
```csharp
private static Results<Ok<ArticleDto>, ValidationProblem, BadRequest<ProblemDetails>> FluentValidationHandler(HttpContext context, IFluentValidationService<ArticleDto> fluentValidationService,
                                                                                                              ArticleDto articleDto) =>
    fluentValidationService.Execute(articleDto)
                           .Map(() => new Article(null, articleDto.Url))
                           .Map(ArticleMapper.MapToDto)
                           .ToWebApi(context, ToOk, ToValidationProblem);

public class ArticleDtoValidator : AbstractValidator<ArticleDto>
{
    public ArticleDtoValidator()
    {
        RuleFor(x => x.Url).NotEmpty().WithMessage("must have a value;");
    }
}
```

### DataAnnotationsValidationService
Calls DataAnnotations validation
```csharp
IDataAnnotationsValidationService dataAnnotationsValidationService
```

e.g.
```csharp
private static Results<Ok<ArticleDto>, ValidationProblem, BadRequest<ProblemDetails>> DataAnnotationsValidationHandler(HttpContext context,
                                                                                                                       IDataAnnotationsValidationService dataAnnotationsValidationService,
                                                                                                                       ArticleDto articleDto) =>
    dataAnnotationsValidationService.Execute(articleDto)
                                    .Map(() => new Article(null, articleDto.Url))
                                    .Map(ArticleMapper.MapToDto)
                                    .ToWebApi(context, ToOk, ToValidationProblem);
```

## Comprehensive samples
There are examples showing the following:
- [x] A basic blog CRUD implementation
- [x] The *ToDo* sample from Damian Edwards [here](https://github.com/DamianEdwards/TrimmedTodo)
- [x] Exception handling
- [x] [Result](https://docs.futurum.dev/dotnet.futurum.core/result/overview.html) error handling
- [x] Validation - DataAnnotations and FluentValidation and both combined

![Comprehensive samples](https://raw.githubusercontent.com/futurum-dev/dotnet.futurum.webapiendpoint.micro.core.extensions/main/docs/Futurum.WebApiEndpoint.Micro.Sample-openapi.png)
