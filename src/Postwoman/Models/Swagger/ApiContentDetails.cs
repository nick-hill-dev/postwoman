using Newtonsoft.Json;

namespace Postwoman.Models.Swagger;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class ApiContentDetails
{

    public Schema Schema { get; set; }

    public object Example { get; set; }

}
