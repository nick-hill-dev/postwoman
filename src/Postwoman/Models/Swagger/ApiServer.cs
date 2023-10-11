using Newtonsoft.Json;

namespace Postwoman.Models.Swagger;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class ApiServer
{

    public string Url { get; set; }

    public string Description { get; set; }

}
