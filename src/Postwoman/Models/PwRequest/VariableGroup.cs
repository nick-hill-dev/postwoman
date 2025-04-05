using Newtonsoft.Json;
using System.Collections.Generic;

namespace Postwoman.Models.PwRequest;

public class VariableGroup
{

    public string Name { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string Inherits { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public List<Variable> Variables { get; set; } = [];

}
