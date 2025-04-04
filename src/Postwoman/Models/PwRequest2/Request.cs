using System.Collections.Generic;

namespace Postwoman.Models.PwRequest2;

public class Request
{

    public string Name { get; set; }

    public string Method { get; set; }

    public string Url { get; set; }

    public RequestAuthorization Authorization { get; set; }

    public List<RequestHeader> Headers { get; set; } = [];

    public List<RequestParameter> Query { get; set; } = [];

    public string Body { get; set; }

}
