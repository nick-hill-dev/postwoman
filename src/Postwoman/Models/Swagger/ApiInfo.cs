using Newtonsoft.Json;

namespace Postwoman.Models.Swagger;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class ApiInfo
{

    public string Title { get; set; }

    public string Version { get; set; }

}