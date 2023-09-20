using Postwoman.Models.PwRequest;
using System.Windows;

namespace Postwoman;

/// <summary>
/// Interaction logic for CollectionHeadersWindow.xaml
/// </summary>
public partial class CollectionHeadersWindow : Window
{

    public CollectionHeadersWindow()
    {
        InitializeComponent();
    }

    public CollectionHeadersWindow(CollectionViewModel collection)
    {
        InitializeComponent();
        DataContext = collection;
    }

}
