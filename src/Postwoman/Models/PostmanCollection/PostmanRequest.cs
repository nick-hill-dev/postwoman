using System.Collections.Generic;

namespace Postwoman.Models.PostmanCollection;

public class PostmanRequest
{

    public string Id { get; set; }

    public string Uid { get; set; }

    public string CollectionId { get; set; }

    public string Name { get; set; }

    public string Url { get; set; }

    public string Description { get; set; }

    public List<PostmanDataItem> Data { get; set; } = new();

    public PostmanDataOptions DataOptions { get; set; }

    public string DataMode { get; set; }

    public List<PostmanHeaderDataItem> HeaderData { get; set; } = new();

    public string Headers { get; set; }

    public string Method { get; set; }

    public List<object> PathVariableData { get; set; } = new();

    public List<PostmanQueryParam> QueryParams { get; set; } = new();

    public PostmanAuth Auth { get; set; }

    public List<PostmanEvent> Events { get; set; } = new();

    public object Folder { get; set; }

    public List<object> ResponsesOrder { get; set; } = new();

    public string CurrentHelper { get; set; }

    public Dictionary<string, string> HelperAttributes { get; set; } = new();

    public object PathVariables { get; set; }

    public List<PostmanVariable> Variables { get; set; } = new();

    public List<string> Order { get; set; } = new();

    public List<object> FoldersOrder { get; set; } = new();

    public object ProtocolProfileBehavior { get; set; }

    public string CreatedAt { get; set; }

    public List<object> Folders { get; set; } = new();

    public string RawModeData { get; set; }

    public List<PostmanRequest> Requests { get; set; } = new();

}