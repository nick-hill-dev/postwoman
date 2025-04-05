using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Postwoman.Models.PwRequestViewModel;

public class ResponseViewModel : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;

    public ObservableCollection<ResponseHeaderViewModel> Headers { get; set; } = [];

    private int _statusCode;

    public int StatusCode
    {
        get => _statusCode;
        set
        {
            _statusCode = value;
            OnPropertyChanged();
        }
    }

    private string _body;

    public string Body
    {
        get => _body;
        set
        {
            _body = value;
            OnPropertyChanged();
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
