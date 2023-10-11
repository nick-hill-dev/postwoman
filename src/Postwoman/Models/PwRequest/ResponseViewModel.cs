using System.Collections.ObjectModel;

namespace Postwoman.Models.PwRequest;

public class ResponseViewModel
{

    public ObservableCollection<ResponseHeaderViewModel> Headers { get; set; } = new();

    public int StatusCode { get; set; }

    public string Body { get; set; }

}
