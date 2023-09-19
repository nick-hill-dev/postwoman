using Postwoman.Models.PwRequest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Postwoman;

public static class VariableReplacer
{

    public static string Replace(string value, ICollection<VariableViewModel> variables)
    {
        var keepGoing = true;
        while (keepGoing)
        {
            keepGoing = false;
            var i1 = value.IndexOf("{{");
            if (i1 >= 0)
            {
                var i2 = value.IndexOf("}}", i1);
                if (i2 > i1)
                {
                    var variableName = value.Substring(i1 + 2, i2 - i1 - 2);
                    var variableValue = variables.FirstOrDefault(v => v.Name == variableName)
                        ?? throw new Exception("Variable not defined: " + variableName);
                    value = value.Replace("{{" + variableName + "}}", variableValue.Value);
                    keepGoing = true;
                }
            }
        }
        return value;
    }

}
