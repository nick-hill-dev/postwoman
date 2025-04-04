using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Postwoman.Models.PwRequestViewModel;

public class CollectionsViewModel : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;

    public ObservableCollection<CollectionViewModel> Collections { get; set; } = [];

    private CollectionViewModel _selectedCollection;

    public CollectionViewModel SelectedCollection
    {
        get
        {
            return _selectedCollection;
        }
        set
        {
            _selectedCollection = value;
            OnPropertyChanged();
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
