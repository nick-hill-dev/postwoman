using Newtonsoft.Json;

namespace Postwoman.Models.PwRequest;

public class RequestAction
{

    public string When { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public SetVariableFromJsonResponseAction SetVariableFromJsonResponse { get; set; }

}
