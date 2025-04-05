using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Postwoman.Models.PwRequestViewModel;

public class VariableGroupViewModel : INotifyPropertyChanged
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

    private VariableGroupViewModel _inherits;

    public VariableGroupViewModel Inherits
    {
        get => _inherits;
        set
        {
            _inherits = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<VariableViewModel> Variables { get; set; } = new();

    private VariableViewModel _selectedVariable;

    public VariableViewModel SelectedVariable
    {
        get => _selectedVariable;
        set
        {
            if (_selectedVariable != value)
            {
                _selectedVariable = value;
                OnPropertyChanged();
            }
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public VariableGroupViewModel Clone(string newName)
    {
        return new VariableGroupViewModel
        {
            Name = newName,
            Inherits = Inherits,
            Variables = [..Variables.Select(v => new VariableViewModel
            {
                Name = v.Name,
                Value = v.Value
            })]
        };
    }

}
