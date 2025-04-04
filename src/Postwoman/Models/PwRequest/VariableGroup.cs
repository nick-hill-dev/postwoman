using System.Collections.Generic;

namespace Postwoman.Models.PwRequest;

public class VariableGroup
{

    public string Name { get; set; }

    public string Inherits { get; set; }

    public List<Variable> Variables { get; set; } = [];

}
