using Newtonsoft.Json;

namespace Postwoman.Models.Swagger;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class ApiContentDetails
{

    public object Schema { get; set; }

    public object Example { get; set; }

}
