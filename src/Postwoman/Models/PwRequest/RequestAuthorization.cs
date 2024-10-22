using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Postwoman.Models.PwRequest;

public class RequestAuthorization : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;

    private string _type = "None";

    public string Type
    {
        get
        {
            return _type;
        }
        set
        {
            _type = value;
            OnPropertyChanged();
        }
    }

    private string _basicUserName;

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string BasicUserName
    {
        get
        {
            return _basicUserName;
        }
        set
        {
            _basicUserName = value;
            OnPropertyChanged();
        }
    }

    private string _basicPassword;

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string BasicPassword
    {
        get
        {
            return _basicPassword;
        }
        set
        {
            _basicPassword = value;
            OnPropertyChanged();
        }
    }

    private string _apiKeyHeaderName = "x-api-key";

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string ApiKeyHeaderName
    {
        get
        {
            return _apiKeyHeaderName;
        }
        set
        {
            _apiKeyHeaderName = value;
            OnPropertyChanged();
        }
    }

    private string _apiKeyValue;

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string ApiKeyValue
    {
        get
        {
            return _apiKeyValue;
        }
        set
        {
            _apiKeyValue = value;
            OnPropertyChanged();
        }
    }

    private string _bearerToken;

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string BearerToken
    {
        get
        {
            return _bearerToken;
        }
        set
        {
            _bearerToken = value;
            OnPropertyChanged();
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}