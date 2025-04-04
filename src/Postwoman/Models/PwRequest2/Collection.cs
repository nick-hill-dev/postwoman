using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Postwoman.Models.PwRequest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Postwoman.Models.PwRequest2;

public class Collection
{

    public string Name { get; set; }

    public List<Server> Servers { get; set; } = [];

    public List<VariableGroup> VariableGroups { get; set; } = [];

    [Obsolete("Don't use this. It remains in place to support upgrading older files that use the old format.")]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public List<Variable> Variables { get; set; } = [];

    public List<EnvironmentInfo> Environments { get; set; } = [];

    public List<RequestHeader> Headers = [];

    public List<Request> Requests = [];

    private static readonly JsonSerializerSettings _serializationSettings = new()
    {
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy(),
        },
        Formatting = Formatting.Indented
    };


    public static Collection Load(string fileName)
    {
        var text = File.ReadAllText(fileName);
        var collection = JsonConvert.DeserializeObject<Collection>(text, _serializationSettings);

        // Upgrade older collections which only have a single variable group
        if (collection.Variables != null)
        {
            var newVariableGroup = new VariableGroup { Name = "Variables", Variables = collection.Variables };
            collection.VariableGroups.Add(newVariableGroup);
            if (collection.Servers.Count > 0)
            {
                collection.Environments.Add(new EnvironmentInfo
                {
                    Name = "Main",
                    Server = collection.Servers.First().Name,
                    VariableGroup = newVariableGroup.Name
                });
            }
            collection.Variables = null;
        }

        // Upgrade older versions where variable sourcing was not previously available
        foreach (var group in collection.VariableGroups)
        {
            foreach (var variable in group.Variables)
            {
                if (string.IsNullOrEmpty(variable.Source))
                {
                    variable.Source = "Collection";
                }
            }
        }

        return collection;
    }

    public void Save(string fileName)
    {
        var json = JsonConvert.SerializeObject(this, _serializationSettings);
        File.WriteAllText(fileName, json);
    }

    public CollectionViewModel ToViewModel()
    {
        var variableGroups = new ObservableCollection<VariableGroupViewModel>(VariableGroups.Select(g => new VariableGroupViewModel
        {
            Name = g.Name,
            Variables = [..g.Variables.Select(v => new VariableViewModel
            {
                Name = v.Name,
                Value = v.Value,
                Source = v.Source
            })]
        }));

        foreach (var variableGroupInFile in VariableGroups)
        {
            if (!string.IsNullOrEmpty(variableGroupInFile.Inherits))
            {
                variableGroups.First(g => g.Name == variableGroupInFile.Name).Inherits = variableGroups.First(g => g.Name == variableGroupInFile.Inherits);
            }
        }

        var servers = new ObservableCollection<ServerViewModel>(Servers.Select(s => new ServerViewModel
        {
            Name = s.Name,
            BaseUrl = s.BaseUrl
        }));

        return new CollectionViewModel
        {
            Name = Name,
            Servers = servers,
            VariableGroups = variableGroups,
            Environments = [..Environments.Select(e => new EnvironmentViewModel
            {
                Name = e.Name,
                Server = servers.First(s => s.Name == e.Server),
                VariableGroup = variableGroups.First(g => g.Name == e.VariableGroup)
            })],
            Headers = [..Headers.Select(h => new RequestHeaderViewModel
            {
                Name = h.Name,
                Value = h.Value
            })],
            Requests = [..Requests.Select(r => new RequestViewModel
            {
                Name = r.Name,
                Method = r.Method,
                Url = r.Url,
                Authorization = r.Authorization == null ? null : new PwRequest.RequestAuthorization
                {
                    Type = r.Authorization.Type,
                    BasicUserName = r.Authorization.BasicUserName,
                    BasicPassword = r.Authorization.BasicPassword,
                    ApiKeyHeaderName = r.Authorization.ApiKeyHeaderName,
                    ApiKeyValue = r.Authorization.ApiKeyValue,
                    BearerToken = r.Authorization.BearerToken
                },
                Headers = [..r.Headers.Select(h => new RequestHeaderViewModel
                {
                    Name = h.Name,
                    Value = h.Value
                })],
                Query = [..r.Query.Select(q => new RequestParameterViewModel
                {
                    Name = q.Name,
                    Value = q.Value
                })],
                Body = r.Body
            })]
        };
    }

}
