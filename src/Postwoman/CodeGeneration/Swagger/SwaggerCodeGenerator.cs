using Newtonsoft.Json;
using Postwoman.Models.PwRequest;
using Postwoman.Models.Swagger;

namespace Postwoman.CodeGeneration.Swagger;

public class SwaggerCodeGenerator : ICodeGenerator
{

    public string Generate(CollectionViewModel collection, RequestViewModel request)
    {

        var model = new ApiDefinition
        {
            Info = new ApiInfo
            {
                Title = collection.Name,
                Version = "1.0.0"
            }
        };

        return JsonConvert.SerializeObject(model, Formatting.Indented);
    }

}
