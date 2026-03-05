using System.Text.RegularExpressions;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace OrchardCore.Cms.KtuSaApi.OpenApi;

internal sealed partial class KtuSaOpenApiTransformer : IOpenApiDocumentTransformer
{
    private static readonly Dictionary<string, string> PathParamDescriptions = new()
    {
        ["pageName"] = "Partial or full display text of the static page to retrieve",
        ["saUnit"] = "SA unit identifier. Allowed values: CSA, InfoSA, Vivat_Chemija, InDi, STATIUS, FUMSA, ESA, SHM, VFSA, BRK",
        ["id"] = "Content item ID of the resource",
    };

    private static readonly Dictionary<string, string> TagDescriptions = new()
    {
        ["Activity Reports"] = "Endpoints for retrieving SA unit activity reports",
        ["Articles"] = "Endpoints for retrieving articles and article content",
        ["Contacts"] = "Endpoints for retrieving SA unit member contacts",
        ["Documents"] = "Endpoints for retrieving documents grouped by category",
        ["Events"] = "Endpoints for retrieving events and event content",
        ["Faqs"] = "Endpoints for retrieving frequently asked questions",
        ["Main Contacts"] = "Endpoints for retrieving SA unit primary contact information",
        ["SA Units"] = "Endpoints for retrieving student association unit details",
        ["Sponsors"] = "Endpoints for retrieving sponsors",
        ["Static Pages"] = "Endpoints for retrieving static pages",
    };

    public Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        FixInfo(document);
        FixServerUrls(document);
        FixMissingPathParameters(document);
        FixTags(document);

        return Task.CompletedTask;
    }

    private static void FixInfo(OpenApiDocument document)
    {
        document.Info.Description ??=
            "KTU Student Association API for managing content, events, contacts and more.";
        document.Info.Contact ??= new OpenApiContact
        {
            Name = "KTU SA",
            Url = new Uri("https://ktusa.lt"),
        };
        document.Info.License ??= new OpenApiLicense { Name = "MIT" };
    }

    private static void FixServerUrls(OpenApiDocument document)
    {
        if (document.Servers is null)
        {
            return;
        }

        foreach (var server in document.Servers)
        {
            server.Url = server.Url?.TrimEnd('/');
        }
    }

    private static void FixMissingPathParameters(OpenApiDocument document)
    {
        foreach (var (path, pathItem) in document.Paths)
        {
            var paramNames = RouteParamRegex().Matches(path)
                .Select(m => m.Groups[1].Value)
                .ToList();

            if (paramNames.Count == 0)
            {
                continue;
            }

            if (pathItem.Operations?.Values == null) continue;
            foreach (var operation in pathItem.Operations.Values)
            {
                operation.Parameters ??= [];

                foreach (var paramName in paramNames.Where(paramName => !operation.Parameters.Any(p =>
                             string.Equals(p.Name, paramName, StringComparison.OrdinalIgnoreCase)
                             && p.In == ParameterLocation.Path)))
                {
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = paramName,
                        In = ParameterLocation.Path,
                        Required = true,
                        Description = PathParamDescriptions.GetValueOrDefault(paramName, paramName),
                        Schema = new OpenApiSchema { Type = JsonSchemaType.String },
                    });
                }
            }
        }
    }

    private static void FixTags(OpenApiDocument document)
    {
        foreach (var (name, description) in TagDescriptions)
        {
            if (document.Tags == null) continue;
            var tag = document.Tags.FirstOrDefault(t => t.Name == name);
            if (tag is null)
            {
                document.Tags.Add(new OpenApiTag { Name = name, Description = description });
            }
            else
            {
                tag.Description ??= description;
            }
        }

        if (document.Tags == null) return;

        var sorted = document.Tags.OrderBy(t => t.Name, StringComparer.Ordinal).ToList();
        document.Tags.Clear();
        foreach (var tag in sorted)
        {
            document.Tags.Add(tag);
        }
    }

    [GeneratedRegex(@"\{(\w+)\}")]
    private static partial Regex RouteParamRegex();
}
