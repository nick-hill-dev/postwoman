using Newtonsoft.Json;

namespace Postwoman.Models.Swagger;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class ApiOperationParameter
{

    public string Name { get; set; }

    public string In { get; set; }

    public bool Required { get; set; }

    public Schema Schema { get; set; }

    public string Description { get; set; }

}