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
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<ServerViewModel> _servers = [];

    public ObservableCollection<ServerViewModel> Servers
    {
        get => _servers;
        set
        {
            _servers = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<VariableGroupViewModel> _variableGroups = [];

    public ObservableCollection<VariableGroupViewModel> VariableGroups
    {
        get => _variableGroups;
        set
        {
            _variableGroups = value;
            OnPropertyChanged();
        }
    }

    private VariableGroupViewModel _selectedVariableGroup;

    public VariableGroupViewModel SelectedVariableGroup
    {
        get => _selectedVariableGroup;
        set
        {
            if (_selectedVariableGroup != value)
            {
                if (_selectedVariableGroup != null)
                {
                    _selectedVariableGroup.PropertyChanged -= SelectedVariableGroup_PropertyChanged;
                }

                _selectedVariableGroup = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedVariable));

                if (_selectedVariableGroup != null)
                {
                    _selectedVariableGroup.PropertyChanged += SelectedVariableGroup_PropertyChanged;
                }
            }
        }
    }

    private ObservableCollection<EnvironmentViewModel> _environments = [];

    public ObservableCollection<EnvironmentViewModel> Environments
    {
        get => _environments;
        set
        {
            _environments = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<RequestHeaderViewModel> _headers = [];

    public ObservableCollection<RequestHeaderViewModel> Headers
    {
        get => _headers;
        set
        {
            _headers = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<RequestViewModel> _requests = [];

    public ObservableCollection<RequestViewModel> Requests
    {
        get => _requests;
        set
        {
            _requests = value;
            OnPropertyChanged();
        }
    }

    private RequestViewModel _selectedRequest;

    public RequestViewModel SelectedRequest
    {
        get => _selectedRequest;
        set
        {
            if (_selectedRequest != value)
            {
                _selectedRequest = value;
                OnPropertyChanged();
            }
        }
    }

    private EnvironmentViewModel _selectedEnvironment;

    public EnvironmentViewModel SelectedEnvironment
    {
        get => _selectedEnvironment;
        set
        {
            _selectedEnvironment = value;
            OnPropertyChanged();
        }
    }

    public VariableViewModel SelectedVariable => SelectedVariableGroup?.SelectedVariable;

    private void SelectedVariableGroup_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(VariableGroupViewModel.SelectedVariable))
        {
            OnPropertyChanged(nameof(SelectedVariable));
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
            Requests = [..Requests.Select(r => r.ToFile())]
        };
    }

}
