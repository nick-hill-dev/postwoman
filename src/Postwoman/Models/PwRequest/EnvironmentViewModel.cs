using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Postwoman.Models.PwRequest;

public class EnvironmentViewModel : INotifyPropertyChanged
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

    private ServerViewModel _server;

    [JsonIgnore]
    public ServerViewModel Server
    {
        get
        {
            return _server;
        }
        set
        {
            _server = value;
            ServerName = value?.Name;
            OnPropertyChanged();
        }
    }

    [JsonProperty("server")]
    public string ServerName { get; set; }

    private VariableGroupViewModel _variableGroup;

    [JsonIgnore]
    public VariableGroupViewModel VariableGroup
    {
        get
        {
            return _variableGroup;
        }
        set
        {
            _variableGroup = value;
            VariableGroupName = value?.Name;
            OnPropertyChanged();
        }
    }

    [JsonProperty("variableGroup")]
    public string VariableGroupName { get; set; }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
