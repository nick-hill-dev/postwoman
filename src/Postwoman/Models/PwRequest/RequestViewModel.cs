using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

    private RequestAuthorization _authorization = new();

    public RequestAuthorization Authorization
    {
        get
        {
            return _authorization;
        }
        set
        {
            _authorization = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<RequestHeaderViewModel> Headers { get; set; } = new();

    public ObservableCollection<RequestParameterViewModel> Query { get; set; } = new();

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

    public RequestViewModel Clone(string newName)
    {
        return new RequestViewModel
        {
            Name = newName,
            Method = Method,
            Url = Url,
            Authorization = Authorization == null ? null : new RequestAuthorization
            {
                Type = Authorization.Type,
                BasicUserName = Authorization.BasicUserName,
                BasicPassword = Authorization.BasicPassword,
                ApiKeyHeaderName = Authorization.ApiKeyHeaderName,
                ApiKeyValue = Authorization.ApiKeyValue,
                BearerToken = Authorization.BearerToken
            },
            Headers = [..Headers.Select(h => new RequestHeaderViewModel
            {
                Name = h.Name,
                Value = h.Value
            })],
            Query = [..Query.Select(h => new RequestParameterViewModel
            {
                IsChecked = h.IsChecked,
                Name = h.Name,
                Value = h.Value
            })],
            Body = Body
        };
    }

}
