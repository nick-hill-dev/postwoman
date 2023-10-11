using Newtonsoft.Json;

namespace Postwoman.Models.Swagger;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class ApiSecurityScheme
{

    public string Type { get; set; }

    public string In { get; set; }

    public string Scheme { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

}
