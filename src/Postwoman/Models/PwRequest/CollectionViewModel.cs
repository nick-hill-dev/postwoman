using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Postwoman.Models.PwRequest;

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

    private ObservableCollection<ServerViewModel> _servers = new();

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

    private ObservableCollection<VariableGroupViewModel> _variableGroups = new();

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

    [JsonIgnore]
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

    private ObservableCollection<VariableViewModel> _variables;

    [Obsolete("Don't use this. It remains in place to support upgrading older files that use the old format.")]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public ObservableCollection<VariableViewModel> Variables
    {
        get
        {
            return _variables;
        }
        set
        {
            _variables = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<EnvironmentViewModel> _environments = new();

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

    private ObservableCollection<RequestHeaderViewModel> _headers = new();

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

    private ObservableCollection<RequestViewModel> _requests = new();

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

    [JsonIgnore]
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

    [JsonIgnore]
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

}
