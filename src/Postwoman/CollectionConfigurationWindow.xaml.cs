using Postwoman.Models.PwRequest;
using System.Windows;

namespace Postwoman;

/// <summary>
/// Interaction logic for CollectionConfigurationWindow.xaml
/// </summary>
public partial class CollectionConfigurationWindow : Window
{

    public CollectionConfigurationWindow()
    {
        InitializeComponent();
    }

    public CollectionConfigurationWindow(CollectionViewModel collection)
    {
        InitializeComponent();
        DataContext = collection;
    }

}
