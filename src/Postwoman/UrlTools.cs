using Postwoman.Models.PwRequest;
using System.Linq;

namespace Postwoman;

public static class UrlTools
{

    public static string GetFullUrl(CollectionViewModel collection, RequestViewModel request)
    {
        var environment = collection.SelectedEnvironment;
        var server = environment?.Server;

        var url = server == null || request.Url.StartsWith("http://") || request.Url.StartsWith("https://")
            ? request.Url
            : server.BaseUrl + (request.Url.StartsWith("/") ? string.Empty : "/") + request.Url;

        var parameters = request.Query.Where(q => q.IsChecked).ToArray();
        if (parameters.Length > 0)
        {
            var i = 0;
            foreach (var parameter in parameters)
            {
                url += (i++ == 0 ? "?" : "&");
                url += $"{parameter.Name}={parameter.Value}";
            }
        }

        var variables = environment?.VariableGroup != null ? VariableCompiler.Compile(environment.VariableGroup) : new();
        return VariableReplacer.Replace(url, variables);
    }

    public static string[] GetFragments(string url)
    {
        var index = url.IndexOf('?');
        return VariableReplacer.Find(index == -1 ? url : url[..index]);
    }

}
