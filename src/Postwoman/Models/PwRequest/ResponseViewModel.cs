using System.Collections.ObjectModel;

namespace Postwoman.Models.PwRequest;

public class ResponseViewModel
{

    public ObservableCollection<ResponseHeaderViewModel> Headers { get; set; } = new();

    public string Body { get; set; }

}
