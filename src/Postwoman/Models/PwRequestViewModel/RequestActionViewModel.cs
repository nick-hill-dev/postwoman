using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Postwoman.Models.PwRequestViewModel;

public class RequestActionViewModel : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;

    private string _when = "AfterResponse";

    public string When
    {
        get => _when;
        set
        {
            _when = value;
            OnPropertyChanged();
        }
    }

    private string _action = "SetVariable";

    public string Action
    {
        get => _action;
        set
        {
            _action = value;
            OnPropertyChanged();
        }
    }

    private string _variableName = string.Empty;

    public string VariableName
    {
        get => _variableName;
        set
        {
            _variableName = value;
            OnPropertyChanged();
        }
    }

    private string _propertyName = string.Empty;

    public string PropertyName
    {
        get => _propertyName;
        set
        {
            _propertyName = value;
            OnPropertyChanged();
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
