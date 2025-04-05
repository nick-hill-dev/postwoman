using Newtonsoft.Json;
using Postwoman.Models.PwRequestViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Postwoman.Models.PwRequest;

public class Request
{

    public string Name { get; set; }

    public string Method { get; set; }

    public string Url { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public RequestAuthorization Authorization { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public List<RequestHeader> Headers { get; set; } = [];

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public List<RequestParameter> Query { get; set; } = [];

    public string Body { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public List<RequestAction> Actions { get; set; } = [];

    public RequestViewModel ToViewModel()
    {
        return new RequestViewModel
        {
            Name = Name,
            Method = Method,
            Url = Url,
            Authorization = Authorization == null ? null : new PwRequestViewModel.RequestAuthorization
            {
                Type = Authorization.Type,
                BasicUserName = Authorization.BasicUserName,
                BasicPassword = Authorization.BasicPassword,
                ApiKeyHeaderName = Authorization.ApiKeyHeaderName,
                ApiKeyValue = Authorization.ApiKeyValue,
                BearerToken = Authorization.BearerToken
            },
            Headers = [..Headers.Select(h => new RequestHeaderViewModel
            {
                Name = h.Name,
                Value = h.Value
            })],
            Query = [..Query.Select(q => new RequestParameterViewModel
            {
                Name = q.Name,
                Value = q.Value
            })],
            Body = Body,
            Actions = [..Actions.Select(a => new RequestActionViewModel
            {
                When = a.When,
                Action = a.SetVariableFromJsonResponse != null ? "SetVariable" : "Error",
                VariableName = a.SetVariableFromJsonResponse?.VariableName,
                PropertyName = a.SetVariableFromJsonResponse?.JsonPath
            })]
        };
    }
}
