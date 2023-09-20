using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Postwoman.Models.PwRequest;

public class RequestViewModel : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;

    private string _name = string.Empty;

    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    private string _method = "GET";

    public string Method
    {
        get
        {
            return _method;
        }
        set
        {
            _method = value;
            OnPropertyChanged();
        }
    }

    private string _url = string.Empty;

    public string Url
    {
        get
        {
            return _url;
        }
        set
        {
            _url = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<RequestHeaderViewModel> Headers { get; set; } = new();

    private string _body = string.Empty;

    public string Body
    {
        get
        {
            return _body;
        }
        set
        {
            _body = value;
            OnPropertyChanged();
        }
    }

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
