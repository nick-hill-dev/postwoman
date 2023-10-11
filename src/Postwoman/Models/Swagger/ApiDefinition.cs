using Newtonsoft.Json;
using System.Collections.Generic;

namespace Postwoman.Models.Swagger;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class ApiDefinition
{

    [JsonProperty("openapi")]
    public string OpenApi { get; set; } = "3.0.0";

    public ApiInfo Info { get; set; }

    public List<ApiServer> Servers { get; set; } = new();

    public ApiComponents Components { get; set; }

    public ApiPaths Paths { get; set; }

}