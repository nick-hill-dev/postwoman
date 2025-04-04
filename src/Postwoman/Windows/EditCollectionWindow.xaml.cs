using Postwoman.Models.PwRequest;
using System.Windows;

namespace Postwoman.Windows;

/// <summary>
/// Interaction logic for EditCollectionWindow.xaml
/// </summary>
public partial class EditCollectionWindow : Window
{

    public EditCollectionWindow()
    {
        InitializeComponent();
    }

    public EditCollectionWindow(CollectionViewModel model)
    {
        InitializeComponent();
        DataContext = model;
    }

}
