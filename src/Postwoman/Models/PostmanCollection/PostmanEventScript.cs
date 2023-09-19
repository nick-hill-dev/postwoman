using System.Collections.Generic;

namespace Postwoman.Models.PostmanCollection;

public class PostmanEventScript
{

    public string Id { get; set; }

    public string Type { get; set; }

    public List<string> Exec { get; set; } = new();

}