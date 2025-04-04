using System.Windows;

namespace Postwoman.Windows;

/// <summary>
/// Interaction logic for NewCollectionWindow.xaml
/// </summary>
public partial class NewCollectionWindow : Window
{

    public string CollectionName
    {
        get { return NameTextBox.Text; }
    }

    public NewCollectionWindow()
    {
        InitializeComponent();
        Loaded += NewCollectionWindow_Loaded;
    }

    private void NewCollectionWindow_Loaded(object sender, RoutedEventArgs e)
    {
        NameTextBox.SelectAll();
        NameTextBox.Focus();
    }

    private void OKButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }

}
