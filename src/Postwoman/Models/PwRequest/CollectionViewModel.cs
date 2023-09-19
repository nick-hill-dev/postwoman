using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Postwoman.Models.PwRequest;

public class CollectionViewModel : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;

    public string Name { get; set; }

    public ObservableCollection<VariableViewModel> Variables { get; set; } = new();

    public ObservableCollection<RequestViewModel> Requests { get; set; } = new();

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
