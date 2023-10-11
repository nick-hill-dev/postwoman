using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Postwoman.Models.PwRequest;
using Postwoman.Models.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        AddOperation(collection, request, model);

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

    private static void AddOperation(CollectionViewModel collection, RequestViewModel request, ApiDefinition model)
    {
        var operation = new ApiOperation
        {
            OperationId = request.Name,
            Summary = request.Name,
            Description = request.Name
        };

        AddOperationSecurity(request, operation);
        AddOperationParameters(collection, request, operation);

        if (!string.IsNullOrEmpty(request.Body))
        {
            operation.RequestBody = new ApiOperationRequestBody
            {
                Required = true,
                Content = new ApiContent
                {
                    {
                        "application/json",
                        new ApiContentDetails
                        {
                            Example = JsonConvert.DeserializeObject(request.Body)
                        }
                    }
                }
            };
        }

        if (!string.IsNullOrEmpty(request.LatestResponse.Body))
        {
            operation.Responses = new ApiOperationResponses
            {
                {
                    request.LatestResponse.StatusCode,
                    new ApiOperationResponse
                    {
                         Description = request.LatestResponse.StatusCode.ToString().StartsWith("2") ? "Successful response." : $"Response for {request.LatestResponse.StatusCode}.",
                         Content = new ApiContent
                         {
                             {
                                 "application/json",
                                 new ApiContentDetails
                                 {
                                     Example = JsonConvert.DeserializeObject(request.LatestResponse.Body)
                                 }
                             }
                         }
                    }
                }
            };
        }

        static string replacer(string variableName)
        {
            return "{" + variableName + "}";
        }
        var url = VariableReplacer.Replace(request.Url, replacer);

        var relativeUrl = request.Url.StartsWith("http://") || request.Url.StartsWith("https://")
            ? new Uri(url).AbsolutePath
            : url.StartsWith("/") ? url : "/" + url;

        model.Paths[relativeUrl] = new ApiPath
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

    private static void AddOperationParameters(CollectionViewModel collection, RequestViewModel request, ApiOperation operation)
    {
        var fullUrl = new Uri(UrlTools.GetFullUrl(collection, request));
        var fragments = UrlTools.GetFragments(request.Url);
        var query = HttpUtility.ParseQueryString(fullUrl.Query);
        if (fragments.Length > 0 || query.Count > 0)
        {
            operation.Parameters = new List<ApiOperationParameter>();
            foreach (var fragment in fragments)
            {
                operation.Parameters.Add(new ApiOperationParameter
                {
                    Name = fragment,
                    In = "path",
                    Required = true,
                    Schema = new
                    {
                        type = GuessType(collection.Variables.First(v => v.Name == fragment).Value)
                    },
                    Description = $"The {fragment} specified in the URL."
                });
            }
            foreach (var queryKey in query.AllKeys)
            {
                operation.Parameters.Add(new ApiOperationParameter
                {
                    Name = queryKey,
                    In = "query",
                    Required = true,
                    Schema = new
                    {
                        type = GuessType(query[queryKey])
                    },
                    Description = $"The {queryKey} specified in the query string."
                });
            }
        }
    }

    private static string GuessType(string value)
    {
        if (double.TryParse(value, out _))
        {
            return "number";
        }
        if (bool.TryParse(value, out _))
        {
            return "boolean";
        }
        return "string";
    }

}
