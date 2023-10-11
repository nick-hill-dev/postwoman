using Postwoman.Models.PwRequest;
using System.Linq;

namespace Postwoman;

public class UrlTools
{

    public static string GetFullUrl(CollectionViewModel collection, RequestViewModel request)
    {
        var url = collection.Servers?.Count == 0 || request.Url.StartsWith("http://") || request.Url.StartsWith("https://")
            ? request.Url
            : collection.Servers.First().BaseUrl + (request.Url.StartsWith("/") ? string.Empty : "/") + request.Url;

        return VariableReplacer.Replace(url, collection.Variables);
    }

    public static string[] GetFragments(string url)
    {
        var index = url.IndexOf('?');
        return VariableReplacer.Find(index == -1 ? url : url[..index]);
    }

}
