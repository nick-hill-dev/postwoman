using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Postwoman.Models.PwRequestViewModel;

public class VariableViewModel : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;

    private string _name = string.Empty;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    private string _value = string.Empty;

    public string Value
    {
        get => _value;
        set
        {
            _value = value;
            OnPropertyChanged();
        }
    }

    private string _source = "Collection";

    public string Source
    {
        get => _source;
        set
        {
            _source = value;
            OnPropertyChanged();
        }
    }

    private string _environmentVariableName = string.Empty;

    public string EnvironmentVariableName
    {
        get => _environmentVariableName;
        set
        {
            _environmentVariableName = value;
            OnPropertyChanged();
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
