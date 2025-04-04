using Postwoman.Models.PwRequestViewModel;
using System;
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
                    switch (variable.Source)
                    {
                        case "Environment":
                            result[variable.Name] = Environment.GetEnvironmentVariable(variable.EnvironmentVariableName);
                            break;

                        default:
                            result[variable.Name] = variable.Value;
                            break;
                    }
                }
            }
            group = group.Inherits;
        }
        return result;
    }

}
