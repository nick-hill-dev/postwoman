using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Postwoman.Models.PwRequest;

public class RequestViewModel : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;

    public string Name { get; set; }

    public string Method { get; set; } = "GET";

    public string Url { get; set; }

    public ObservableCollection<RequestHeaderViewModel> Headers { get; set; } = new();

    public string Body { get; set; }

    private ResponseViewModel _latestResponse;

    [JsonIgnore]
    public ResponseViewModel LatestResponse
    {
        get
        {
            return _latestResponse;
        }
        set
        {
            _latestResponse = value;
            OnPropertyChanged();
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
