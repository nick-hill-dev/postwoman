using System.Collections.Generic;

namespace Postwoman.Models.PostmanCollection;

public class PostmanAuth
{

    public string Type { get; set; }

    public List<PostmanAuthKeyValue> Basic { get; set; } = new();

    public List<PostmanAuthKeyValue> Bearer { get; set; } = new();

    public List<PostmanAuthKeyValue> ApiKey { get; set; } = new();

}