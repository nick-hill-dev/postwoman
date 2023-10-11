using Postwoman.Models.PwRequest;

namespace Postwoman.CodeGeneration;

public interface ICodeGenerator
{

    string Generate(CollectionViewModel collection, RequestViewModel request);

}
