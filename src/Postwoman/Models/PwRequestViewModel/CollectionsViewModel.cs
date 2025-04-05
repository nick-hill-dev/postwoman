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
        get => _selectedCollection;
        set
        {
            if (_selectedCollection != value)
            {
                if (_selectedCollection != null)
                {
                    _selectedCollection.PropertyChanged -= SelectedCollection_PropertyChanged;
                }

                _selectedCollection = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedRequest));

                if (_selectedCollection != null)
                {
                    _selectedCollection.PropertyChanged += SelectedCollection_PropertyChanged;
                }
            }
        }
    }

    public RequestViewModel SelectedRequest => SelectedCollection?.SelectedRequest;

    private void SelectedCollection_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(CollectionViewModel.SelectedRequest))
        {
            OnPropertyChanged(nameof(SelectedRequest));
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
