using Postwoman.Models.PwRequest;
using System.Windows;

namespace Postwoman;

/// <summary>
/// Interaction logic for CollectionVariablesWindow.xaml
/// </summary>
public partial class CollectionVariablesWindow : Window
{

    public CollectionVariablesWindow()
    {
        InitializeComponent();
    }

    public CollectionVariablesWindow(CollectionViewModel collection)
    {
        InitializeComponent();
        DataContext = collection;
    }

}
