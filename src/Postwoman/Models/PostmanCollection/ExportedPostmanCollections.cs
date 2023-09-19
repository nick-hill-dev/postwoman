using System.Collections.Generic;

namespace Postwoman.Models.PostmanCollection;

public class ExportedPostmanCollections
{

    public int Version { get; set; }

    public List<PostmanRequest> Collections { get; set; } = new List<PostmanRequest>();

}
