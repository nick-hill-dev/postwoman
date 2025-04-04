using Postwoman.Models.PwRequest;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Postwoman.Models.PwRequestViewModel;

public class CollectionViewModel : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;

    private string _name;

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

    private ObservableCollection<ServerViewModel> _servers = [];

    public ObservableCollection<ServerViewModel> Servers
    {
        get
        {
            return _servers;
        }
        set
        {
            _servers = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<VariableGroupViewModel> _variableGroups = [];

    public ObservableCollection<VariableGroupViewModel> VariableGroups
    {
        get
        {
            return _variableGroups;
        }
        set
        {
            _variableGroups = value;
            OnPropertyChanged();
        }
    }

    private VariableGroupViewModel _selectedVariableGroup;

    public VariableGroupViewModel SelectedVariableGroup
    {
        get
        {
            return _selectedVariableGroup;
        }
        set
        {
            _selectedVariableGroup = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<EnvironmentViewModel> _environments = [];

    public ObservableCollection<EnvironmentViewModel> Environments
    {
        get
        {
            return _environments;
        }
        set
        {
            _environments = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<RequestHeaderViewModel> _headers = [];

    public ObservableCollection<RequestHeaderViewModel> Headers
    {
        get
        {
            return _headers;
        }
        set
        {
            _headers = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<RequestViewModel> _requests = [];

    public ObservableCollection<RequestViewModel> Requests
    {
        get
        {
            return _requests;
        }
        set
        {
            _requests = value;
            OnPropertyChanged();
        }
    }

    private RequestViewModel _selectedRequest;

    public RequestViewModel SelectedRequest
    {
        get
        {
            return _selectedRequest;
        }
        set
        {
            _selectedRequest = value;
            OnPropertyChanged();
        }
    }

    private EnvironmentViewModel _selectedEnvironment;

    public EnvironmentViewModel SelectedEnvironment
    {
        get
        {
            return _selectedEnvironment;
        }
        set
        {
            _selectedEnvironment = value;
            OnPropertyChanged();
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public Collection ToFile()
    {
        return new Collection
        {
            Name = Name,
            Servers = [.. Servers.Select(s => new Server
            {
                Name = s.Name,
                BaseUrl = s.BaseUrl
            })],
            VariableGroups = [..VariableGroups.Select(g => new VariableGroup
            {
                Name = g.Name,
                Inherits = g.Inherits?.Name,
                Variables = [..g.Variables.Select(v => new Variable
                {
                    Name = v.Name,
                    Value = v.Source == "Temporary" ? null: v.Value,
                    Source = v.Source,
                    EnvironmentVariableName = v.EnvironmentVariableName
                })]
            })],
            Environments = [..Environments.Select(e => new EnvironmentInfo
            {
                Name = e.Name,
                Server = e.Server?.Name,
                VariableGroup = e.VariableGroup?.Name
            })],
            Headers = [..Headers.Select(h => new RequestHeader
            {
                Name = h.Name,
                Value = h.Value
            })],
            Requests = [..Requests.Select(r => new Request
            {
                Name = r.Name,
                Method = r.Method,
                Url = r.Url,
                Authorization = r.Authorization == null ? null : new PwRequest.RequestAuthorization
                {
                    Type = r.Authorization.Type,
                    BasicUserName = r.Authorization.BasicUserName,
                    BasicPassword = r.Authorization.BasicPassword,
                    ApiKeyHeaderName = r.Authorization.ApiKeyHeaderName,
                    ApiKeyValue = r.Authorization.ApiKeyValue,
                    BearerToken = r.Authorization.BearerToken
                },
                Headers = [..r.Headers.Select(h => new RequestHeader
                {
                    Name = h.Name,
                    Value = h.Value
                })],
                Query = [..r.Query.Select(q => new RequestParameter
                {
                    Name = q.Name,
                    Value = q.Value
                })],
                Body = r.Body
            })]
        };
    }

}
