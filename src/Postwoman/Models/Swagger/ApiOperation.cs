using Newtonsoft.Json;
using System.Collections.Generic;

namespace Postwoman.Models.Swagger;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class ApiOperation
{

    public string OperationId { get; set; }

    public string Summary { get; set; }

    public string Description { get; set; }

    public List<ApiOperationSecurity> Security { get; set; }

    public List<ApiOperationParameter> Parameters { get; set; }

    public ApiOperationRequestBody RequestBody { get; set; }

    public ApiOperationResponses Responses { get; set; }

}