using FluentValidation;

namespace Futurum.WebApiEndpoint.Micro.Core.Extensions.Sample.Blog;

public class BlogDtoValidator : AbstractValidator<BlogDto>
{
    public BlogDtoValidator()
    {
        RuleFor(x => x.Url).NotEmpty().WithMessage("must have a value;");
    }
}