using Newtonsoft.Json;

namespace Postwoman.Models.Swagger;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class ApiOperationResponse
{

    public string Description { get; set; }

    public ApiContent Content { get; set; }

}