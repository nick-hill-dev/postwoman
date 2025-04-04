using Postwoman.Models.PwRequestViewModel;

namespace Postwoman.CodeGeneration;

public interface ICodeGenerator
{

    string Generate(CollectionViewModel collection, RequestViewModel request);

}
