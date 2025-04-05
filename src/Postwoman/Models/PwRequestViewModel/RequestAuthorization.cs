using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Postwoman.Models.PwRequestViewModel;

public class RequestAuthorization : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;

    private string _type = "None";

    public string Type
    {
        get => _type;
        set
        {
            _type = value;
            OnPropertyChanged();
        }
    }

    private string _basicUserName;

    public string BasicUserName
    {
        get => _basicUserName;
        set
        {
            _basicUserName = value;
            OnPropertyChanged();
        }
    }

    private string _basicPassword;

    public string BasicPassword
    {
        get => _basicPassword;
        set
        {
            _basicPassword = value;
            OnPropertyChanged();
        }
    }

    private string _apiKeyHeaderName = "x-api-key";

    public string ApiKeyHeaderName
    {
        get => _apiKeyHeaderName;
        set
        {
            _apiKeyHeaderName = value;
            OnPropertyChanged();
        }
    }

    private string _apiKeyValue;

    public string ApiKeyValue
    {
        get => _apiKeyValue;
        set
        {
            _apiKeyValue = value;
            OnPropertyChanged();
        }
    }

    private string _bearerToken;

    public string BearerToken
    {
        get => _bearerToken;
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