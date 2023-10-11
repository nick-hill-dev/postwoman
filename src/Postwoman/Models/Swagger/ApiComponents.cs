using Newtonsoft.Json;

namespace Postwoman.Models.Swagger;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class ApiComponents
{

    public ApiSecuritySchemes SecuritySchemes { get; set; }

}
