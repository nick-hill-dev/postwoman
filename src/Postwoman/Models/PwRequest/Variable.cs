using Newtonsoft.Json;

namespace Postwoman.Models.PwRequest;

public class Variable
{

    public string Name { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string Value { get; set; }

    public string Source { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string EnvironmentVariableName { get; set; }

}
