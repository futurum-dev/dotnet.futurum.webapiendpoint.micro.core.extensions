namespace Futurum.WebApiEndpoint.Micro.Core.Extensions.Sample.Blog;

public record Id(long Value);

public static class IdExtensions
{
    public static Id ToId(this long id) =>
        new(id);
}
public record Blog(Option<Id> Id, string Url);