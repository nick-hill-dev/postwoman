using System.Collections.Generic;

namespace Postwoman.Models.PostmanCollection;

public class PostmanAuth
{

    public string Type { get; set; }

    public List<PostmanAuthKeyVaule> Basic { get; set; } = new List<PostmanAuthKeyVaule>();

    public List<PostmanAuthKeyVaule> Bearer { get; set; } = new List<PostmanAuthKeyVaule>();

}