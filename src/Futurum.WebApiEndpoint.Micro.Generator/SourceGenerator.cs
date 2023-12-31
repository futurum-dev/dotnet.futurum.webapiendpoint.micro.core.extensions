﻿using System.Text;
using System.Text.RegularExpressions;

using Futurum.WebApiEndpoint.Micro.Generator.Core;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Futurum.WebApiEndpoint.Micro.Generator;

[Generator]
public class SourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var assemblyName = context.CompilationProvider
                                  .Select(static (c, _) => c.AssemblyName);

        Generator(context, assemblyName);

        FluentValidator(context, assemblyName);
    }

    private static void Generator(IncrementalGeneratorInitializationContext context, IncrementalValueProvider<string?> assemblyName)
    {
        context.RegisterSourceOutput(assemblyName,
                                     static (productionContext, assemblyName) => ExecuteGeneration(productionContext, assemblyName));

        static void ExecuteGeneration(SourceProductionContext context, string assemblyName)
        {
            var methodName = Regex.Replace(assemblyName, "\\W", "");

            var codeBuilder = SourceGeneratorWriter.Write(methodName);

            context.AddSource("Generator.g.cs", SourceText.From(codeBuilder.ToString(), Encoding.UTF8));
        }
    }

    private static void FluentValidator(IncrementalGeneratorInitializationContext context, IncrementalValueProvider<string?> assemblyName)
    {
        var fluentValidatorData = context.SyntaxProvider
                                         .CreateSyntaxProvider(FluentValidatorSourceGenerator.SemanticPredicate, FluentValidatorSourceGenerator.SemanticTransform)
                                         .Where(node => node is not null);

        var fluentValidatorDiagnostics = fluentValidatorData
                                         .Select(static (item, _) => item.Diagnostics)
                                         .Where(static item => item.Count > 0);

        context.RegisterSourceOutput(fluentValidatorDiagnostics, ReportDiagnostic);

        context.RegisterSourceOutput(fluentValidatorData.Collect().Combine(assemblyName),
                                     static (productionContext, source) => FluentValidatorSourceGenerator.ExecuteGeneration(productionContext, source.Left, source.Right));
    }

    private static void ReportDiagnostic(SourceProductionContext context, EquatableArray<Diagnostic> diagnostics)
    {
        foreach (var diagnostic in diagnostics)
            context.ReportDiagnostic(diagnostic);
    }
}
