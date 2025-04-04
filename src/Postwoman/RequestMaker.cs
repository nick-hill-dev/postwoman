using Newtonsoft.Json;
using Postwoman.Models.PwRequest;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Postwoman;

public class RequestMaker
{

    private CollectionViewModel _collection { get; }

    public RequestMaker(CollectionViewModel selectedCollection)
    {
        _collection= selectedCollection;
    }

    public async Task<ResponseViewModel> Send(RequestViewModel requestDetails)
    {
        var requestMethod = GetRequestMethod(requestDetails.Method);

        var fullUrl = UrlTools.GetFullUrl(_collection, requestDetails);

        var request = new HttpRequestMessage(requestMethod, fullUrl);

        var variables = VariableCompiler.Compile(_collection.SelectedEnvironment?.VariableGroup ?? new());
        foreach (var header in _collection.Headers)
        {
            request.Headers.TryAddWithoutValidation(
                header.Name,
                VariableReplacer.Replace(header.Value, variables)
            );
        }

        foreach (var header in requestDetails.Headers)
        {
            request.Headers.TryAddWithoutValidation(
                header.Name,
                VariableReplacer.Replace(header.Value, variables)
            );
        }

        switch (requestDetails.Authorization.Type)
        {
            case "Basic":
                var userName = VariableReplacer.Replace(requestDetails.Authorization.BasicUserName, variables);
                var password = VariableReplacer.Replace(requestDetails.Authorization.BasicPassword, variables);
                var authenticationString = $"{userName}:{password}";
                var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                break;

            case "ApiKey":
                var apiKey = VariableReplacer.Replace(requestDetails.Authorization.ApiKeyValue, variables);
                request.Headers.TryAddWithoutValidation(requestDetails.Authorization.ApiKeyHeaderName, apiKey);
                break;

            case "Bearer":
                var bearerToken = VariableReplacer.Replace(requestDetails.Authorization.BearerToken, variables);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                break;
        }

        if (!string.IsNullOrEmpty(requestDetails.Body))
        {
            var specifiedContentType = requestDetails.Headers.FirstOrDefault(h => h.Name == "Content-Type")?.Value;
            var mediaType = specifiedContentType ?? "application/json";
            request.Content = new StringContent(
                VariableReplacer.Replace(requestDetails.Body, variables),
                Encoding.UTF8,
                mediaType
            );
        }

        var client = new HttpClient();
        var response = await client.SendAsync(request);

        var responseText = await response.Content.ReadAsStringAsync();
        if (response.Content.Headers.ContentType.MediaType == "application/json")
        {
            try
            {
                responseText = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(responseText), Formatting.Indented);
            }
            catch
            {
            }
        }

        return new ResponseViewModel
        {
            Body = responseText,
            StatusCode = (int)response.StatusCode,
            Headers = new ObservableCollection<ResponseHeaderViewModel>(response.Content.Headers.Select(h => new ResponseHeaderViewModel
            {
                Name = h.Key,
                Value = string.Join(", ", h.Value)
            }))
        };
    }

    private static HttpMethod GetRequestMethod(string method)
    {
        var requestMethod = HttpMethod.Get;
        switch (method)
        {
            case "POST":
                requestMethod = HttpMethod.Post;
                break;

            case "PUT":
                requestMethod = HttpMethod.Put;
                break;

            case "DELETE":
                requestMethod = HttpMethod.Delete;
                break;

            case "OPTIONS":
                requestMethod = HttpMethod.Options;
                break;
        }

        return requestMethod;
    }

}
