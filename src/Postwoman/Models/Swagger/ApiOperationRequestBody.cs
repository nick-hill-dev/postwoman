using Newtonsoft.Json;

namespace Postwoman.Models.Swagger;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class ApiOperationRequestBody
{

    public bool Required { get; set; }

    public ApiContent Content { get; set; }

}