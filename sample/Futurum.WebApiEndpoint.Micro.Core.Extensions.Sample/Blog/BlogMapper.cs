namespace Futurum.WebApiEndpoint.Micro.Core.Extensions.Sample.Blog;

public static class BlogMapper
{
    public static BlogDto MapToDto(Blog domain) =>
        new(domain.Id.GetValueOrDefault(x => x.Value, 0), domain.Url);
}