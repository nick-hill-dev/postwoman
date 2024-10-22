using System;
using System.Collections.Generic;
using System.Linq;

namespace Postwoman;

public static class VariableReplacer
{

    public static string[] Find(string value)
    {
        var result = new HashSet<string>();
        var i1 = value.IndexOf("{{");
        while (i1 != -1)
        {
            var i2 = value.IndexOf("}}", i1);
            if (i2 > i1)
            {
                var variableName = value.Substring(i1 + 2, i2 - i1 - 2);
                result.Add(variableName);
                i1 = value.IndexOf("{{", i2);
            }
            else
            {
                i1 = -1;
            }
        }
        return result.ToArray();
    }

    public static string Replace(string value, Func<string, string> replacer)
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
                    var variableValue = replacer(variableName);
                    value = value.Replace("{{" + variableName + "}}", variableValue);
                    keepGoing = true;
                }
            }
        }
        return value;
    }

    public static string Replace(string value, Dictionary<string, string> variables)
    {
        string replacer(string variableName)
        {
            if (!variables.TryGetValue(variableName, out var variableValue))
            {
                throw new Exception("Variable not defined: " + variableName);
            }
            return variableValue;
        }
        return Replace(value, replacer);
    }

}
