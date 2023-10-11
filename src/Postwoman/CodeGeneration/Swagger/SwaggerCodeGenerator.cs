using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Postwoman.Models.PwRequest;
using Postwoman.Models.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Postwoman.CodeGeneration.Swagger;

public class SwaggerCodeGenerator : ICodeGenerator
{

    public string Generate(CollectionViewModel collection, RequestViewModel request)
    {

        var model = new ApiDefinition
        {
            Info = new ApiInfo
            {
                Title = collection.Name,
                Version = "1.0.0"
            },
            Paths = new ApiPaths()
        };

        AddServers(collection, model);
        AddSecurity(request, model);
        AddOperation(request, model);

        var contractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };

        return JsonConvert.SerializeObject(model, new JsonSerializerSettings
        {
            ContractResolver = contractResolver,
            Formatting = Formatting.Indented
        });
    }

    private static void AddServers(CollectionViewModel collection, ApiDefinition model)
    {
        if (collection?.Servers.Count > 0)
        {
            model.Servers = collection.Servers.Select(s => new ApiServer
            {
                Description = s.Name,
                Url = s.BaseUrl
            }).ToList();
        }
    }

    private static void AddSecurity(RequestViewModel request, ApiDefinition model)
    {
        model.Components = new ApiComponents
        {
            SecuritySchemes = new ApiSecuritySchemes
            {
            }
        };

        if (request.Authorization.Type == "Basic")
        {
            model.Components.SecuritySchemes.Add(
                "BasicAuth",
                new ApiSecurityScheme
                {
                    Type = "http",
                    Scheme = "basic",
                    Description = "Basic Auth"
                });
        }

        if (request.Authorization.Type == "ApiKey")
        {
            model.Components.SecuritySchemes.Add(
                "ApiKeyAuth",
                new ApiSecurityScheme
                {
                    Type = "apiKey",
                    In = "header",
                    Name = "x-api-key",
                    Description = "API Key"
                });
        }

        if (request.Authorization.Type == "Bearer")
        {
            model.Components.SecuritySchemes.Add(
                "BearerAuth",
                new ApiSecurityScheme
                {
                    Type = "http",
                    Scheme = "bearer",
                    Description = "JWT Bearer Token"
                });
        }
    }

    private static void AddOperation(RequestViewModel request, ApiDefinition model)
    {
        var operation = new ApiOperation
        {
            OperationId = request.Name,
            Summary = request.Name,
            Description = request.Name
        };

        AddOperationSecurity(request, operation);

        static string replacer(string variableName)
        {
            return "{" + variableName + "}";
        }
        var url = VariableReplacer.Replace(request.Url, replacer);

        model.Paths[url] = new ApiPath
        {
            { request.Method.ToLower(), operation }
        };
    }

    private static void AddOperationSecurity(RequestViewModel request, ApiOperation operation)
    {
        if (request.Authorization.Type == "Basic")
        {
            operation.Security = new List<ApiOperationSecurity>
            {
                new ApiOperationSecurity
                {
                    { "BasicAuth", new List<string>() }
                }
            };
        }

        if (request.Authorization.Type == "ApiKey")
        {
            operation.Security = new List<ApiOperationSecurity>
            {
                new ApiOperationSecurity
                {
                    { "ApiKeyAuth", new List<string>() }
                }
            };
        }

        if (request.Authorization.Type == "Bearer")
        {
            operation.Security = new List<ApiOperationSecurity>
            {
                new ApiOperationSecurity
                {
                    { "BearerAuth", new List<string>() }
                }
            };
        }
    }
}
