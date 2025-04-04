using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Postwoman.CodeGeneration.Swagger;
using Postwoman.Models.PwRequestViewModel;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Linq;
using Postwoman.Models.Swagger;

namespace Postwoman.Windows;

/// <summary>
/// Interaction logic for GenerateCollectionCodeWindow.xaml
/// </summary>
public partial class GenerateCollectionCodeWindow : Window
{

    private readonly CollectionViewModel _collection;

    public GenerateCollectionCodeWindow()
    {
        InitializeComponent();
    }

    public GenerateCollectionCodeWindow(CollectionViewModel collection)
    {
        InitializeComponent();
        _collection = collection;
    }

    private void GenerateButton_Click(object sender, RoutedEventArgs e)
    {
        var contractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };

        var settings = new JsonSerializerSettings
        {
            ContractResolver = contractResolver,
            Formatting = Formatting.Indented
        };

        var rootFileName = "api.json";
        var rootModel = (dynamic)(new
        {
            Paths = new Dictionary<string, object>()
        });
        var files = new Dictionary<string, object>
        {
            [rootFileName] = rootModel
        };

        foreach (var request in _collection.Requests)
        {
            var model = SwaggerCodeGenerator.GenerateSwaggerDefinition(_collection, request);
            rootModel.OpenApi = model.OpenApi;
            rootModel.Info = model.Info;
            rootModel.Servers = model.Servers;
            rootModel.Components = model.Components;

            var requestFileName = request.Name + ".json";

            var requestPath = model.Paths.First();
            rootModel.Paths[requestPath.Key] = new
            {
                reff = requestFileName
            };
            files[requestFileName] = requestPath.Value;
        }

        foreach (var file in files)
        {
            var fileName = Path.Combine(TargetDirectoryTextBox.Text, file.Key);
            var code = JsonConvert.SerializeObject(file.Value, settings);
            File.WriteAllText(fileName, code);
        }

    }

}
