using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Postwoman.Models.PwRequest;
using System.IO;
using System.Linq;

namespace Postwoman;

public class CollectionLoader
{

    public static CollectionViewModel Load(string fileName)
    {
        var serializationSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        };

        var text = File.ReadAllText(fileName);
        var collection = JsonConvert.DeserializeObject<CollectionViewModel>(text, serializationSettings);
        foreach (var group in collection.VariableGroups)
        {
            if (!string.IsNullOrEmpty(group.InheritsGroupName))
            {
                group.Inherits = collection.VariableGroups.First(g => g.Name == group.InheritsGroupName);
            }
        }
        foreach (var environment in collection.Environments)
        {
            if (!string.IsNullOrEmpty(environment.ServerName))
            {
                environment.Server = collection.Servers.First(s => s.Name == environment.ServerName);
            }
            if (!string.IsNullOrEmpty(environment.VariableGroupName))
            {
                environment.VariableGroup = collection.VariableGroups.First(g => g.Name == environment.VariableGroupName);
            }
        }

        // Upgrade older collections which only have a single variable group
        if (collection.Variables != null)
        {
            var newVariableGroup = new VariableGroupViewModel { Name = "Variables", Variables = collection.Variables };
            collection.VariableGroups.Add(newVariableGroup);
            if (collection.Servers.Count > 0)
            {
                collection.Environments.Add(new EnvironmentViewModel { Name = "Main", Server = collection.Servers.First(), VariableGroup = newVariableGroup });
            }
            collection.Variables = null;
        }

        return collection;
    }

}