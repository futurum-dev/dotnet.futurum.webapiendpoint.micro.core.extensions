using Futurum.WebApiEndpoint.Micro.Generator.Core;

namespace Futurum.WebApiEndpoint.Micro.Generator;

public static class WrapperSourceGeneratorWriter
{
    public static string Write(string className, string methodName, Action<IndentedStringBuilder> writer, bool isNotMainMethod, bool skipVersion = false)
    {
        var codeBuilder = new IndentedStringBuilder();
        codeBuilder
            .AppendLine("// <auto-generated />")
            .AppendLine("#nullable enable")
            .AppendLine();

        codeBuilder
            .AppendLine("namespace Microsoft.Extensions.DependencyInjection")
            .AppendLine("{")
            .IncrementIndent();

        if (!isNotMainMethod)
        {
            codeBuilder
                .AppendLine("[global::System.Diagnostics.DebuggerNonUserCodeAttribute]")
                .AppendLine("[global::System.Diagnostics.DebuggerStepThroughAttribute]");
        }

        codeBuilder
            .AppendLine($"public static partial class {className}FuturumWebApiEndpointMicroExtensions")
            .AppendLine("{")
            .IncrementIndent();

        if (!isNotMainMethod)
        {
            codeBuilder
                .Append("public");
        }

        if (isNotMainMethod)
        {
            codeBuilder
                .Append("private");
        }

        codeBuilder
            .Append(" static global::Microsoft.Extensions.DependencyInjection.IServiceCollection")
            .Append(" ")
            .Append(methodName)
            .AppendLine("(this global::Microsoft.Extensions.DependencyInjection.IServiceCollection serviceCollection)")
            .AppendLine("{")
            .IncrementIndent();

        writer(codeBuilder);

        codeBuilder
            .AppendLine("return serviceCollection;")
            .DecrementIndent()
            .AppendLine("}") // method
            .DecrementIndent()
            .AppendLine("}") // class
            .DecrementIndent()
            .AppendLine("}"); // namespace

        return codeBuilder.ToString();
    }
}
