using System;
using System.Text.Json;

namespace Postwoman;

public static class ContentTypeCalculator
{

    public static string Analyze(string content)
    {
        if (IsValidJson(content))
        {
            return "application/json";
        }
        return "text/plain";
    }

    private static bool IsValidJson(string content)
    {
        try
        {
            JsonDocument.Parse(content);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
        catch (ArgumentNullException)
        {
            return false;
        }
    }

}
