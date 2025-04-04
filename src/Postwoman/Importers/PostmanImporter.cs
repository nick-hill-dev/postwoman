﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Postwoman.Models.PostmanCollection;
using Postwoman.Models.PwRequestViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Postwoman.Importers;

public class PostmanImporter
{

    public static List<CollectionViewModel> Import(string fileName)
    {
        var text = File.ReadAllText(fileName);

        var contractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };
        var serializationSettings = new JsonSerializerSettings
        {
            ContractResolver = contractResolver
        };
        var postmanCollection = JsonConvert.DeserializeObject<ExportedPostmanCollections>(text, serializationSettings);

        var result = new List<CollectionViewModel>();
        foreach (var collection in postmanCollection.Collections.OrderBy(c => c.Name))
        {
            var newCollection = new CollectionViewModel
            {
                Name = collection.Name
            };

            if (collection.Variables.Count > 0)
            {
                var variableGroup = new VariableGroupViewModel
                {
                    Name = "Variables",
                    Variables = new ObservableCollection<VariableViewModel>
                    (
                        collection.Variables.Select(v => new VariableViewModel
                        {
                            Name = v.Key,
                            Value = v.Value
                        })
                    )
                };
                newCollection.VariableGroups.Add(variableGroup);

                newCollection.Environments.Add(new EnvironmentViewModel
                {
                    Name = "Main",
                    VariableGroup = variableGroup
                });
            }

            if (collection.Auth?.ApiKey?.Count > 0)
            {
                newCollection.Headers.Add(new RequestHeaderViewModel
                {
                    Name = collection.Auth.ApiKey.First(k => k.Key == "key").Value,
                    Value = collection.Auth.ApiKey.First(k => k.Key == "value").Value
                });
            }

            foreach (var request in collection.Requests)
            {
                var newRequest = new RequestViewModel
                {
                    Method = request.Method,
                    Name = request.Name,
                    Url = request.Url
                };

                if (request.Auth?.ApiKey?.Count > 0)
                {
                    newRequest.Headers.Add(new RequestHeaderViewModel
                    {
                        Name = request.Auth.ApiKey.First(k => k.Key == "key").Value,
                        Value = request.Auth.ApiKey.First(k => k.Key == "value").Value
                    });
                }

                foreach (var header in request.HeaderData ?? new List<PostmanHeaderDataItem>())
                {
                    newRequest.Headers.Add(new RequestHeaderViewModel
                    {
                        Name = header.Key,
                        Value = header.Value
                    });
                }

                if (!string.IsNullOrEmpty(request.RawModeData))
                {
                    newRequest.Body = request.RawModeData;
                }
                newCollection.Requests.Add(newRequest);
            }
            result.Add(newCollection);
        }
        return result;
    }

}
