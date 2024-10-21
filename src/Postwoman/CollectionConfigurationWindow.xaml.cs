using Postwoman.Models.PwRequest;
using System.Windows;

namespace Postwoman;

/// <summary>
/// Interaction logic for CollectionConfigurationWindow.xaml
/// </summary>
public partial class CollectionConfigurationWindow : Window
{

    private readonly CollectionViewModel _collection;

    public CollectionConfigurationWindow()
    {
        InitializeComponent();
    }

    public CollectionConfigurationWindow(CollectionViewModel collection)
    {
        InitializeComponent();
        _collection = collection;
        DataContext = collection;
    }

    private void NewVariableGroupMenuItem_Click(object sender, RoutedEventArgs e)
    {
        var newVariableGroup = new VariableGroupViewModel { Name = "New variable group" };
        _collection.VariableGroups.Add(newVariableGroup);
        _collection.SelectedVariableGroup = newVariableGroup;
    }

    private void DeleteVariableGroupMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show("Are you sure you wish to delete the selected variable group?", "Delete Variable Group", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
            _collection.VariableGroups.Remove(_collection.SelectedVariableGroup);
            _collection.SelectedVariableGroup = null;
        }
    }

    private void DuplicateVariableGroupMenuItem_Click(object sender, RoutedEventArgs e)
    {
        var selectedGroup = _collection.SelectedVariableGroup;
        var newGroup = selectedGroup.Clone("Copy of " + selectedGroup.Name);
        _collection.VariableGroups.Add(newGroup);
        _collection.SelectedVariableGroup = newGroup;
    }

    private void NewEnvironmentMenuItem_Click(object sender, RoutedEventArgs e)
    {
        var newEnvironment = new EnvironmentViewModel { Name = "New environment" };
        _collection.Environments.Add(newEnvironment);
        _collection.SelectedEnvironment = newEnvironment;
    }

    private void DeleteEnvironmentMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show("Are you sure you wish to delete the selected environment?", "Delete Environment", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
            _collection.Environments.Remove(_collection.SelectedEnvironment);
            _collection.SelectedEnvironment = null;
        }
    }

}
