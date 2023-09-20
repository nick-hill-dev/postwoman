using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Postwoman.Models.PwRequest;

public class CollectionViewModel : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;

    public string Name { get; set; }

    private ObservableCollection<VariableViewModel> _variables = new();

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

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
