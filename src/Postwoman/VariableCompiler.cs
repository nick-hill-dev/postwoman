using Postwoman.Models.PwRequest;
using System.Collections.Generic;

namespace Postwoman;

public static class VariableCompiler
{

    public static Dictionary<string, string> Compile(VariableGroupViewModel leafGroup)
    {
        var result = new Dictionary<string, string>();
        var group = leafGroup;
        while (group != null)
        {
            foreach (var variable in group.Variables)
            {
                if (!result.ContainsKey(variable.Name))
                {
                    result[variable.Name] = variable.Value;
                }
            }
            group = group.Inherits;
        }
        return result;
    }

}
