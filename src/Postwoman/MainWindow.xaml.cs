using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Postwoman.Importers;
using Postwoman.Models.PwRequest;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Postwoman
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private JsonSerializerSettings serializationSettings;

        private CollectionsViewModel collections;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            serializationSettings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            };

            collections = new CollectionsViewModel();
            DataContext = collections;
        }

        private void ImportRequestMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "Postwoman Requests (*.pwr)|*.pwr|All Files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == true)
            {
                var text = File.ReadAllText(dialog.FileName);
                var json = JsonConvert.DeserializeObject<RequestViewModel>(text, serializationSettings);
                collections.SelectedCollection.Requests.Add(json);
            }
        }

        private void ExportRequestMenuItem_Click(object sender, RoutedEventArgs e)
        {
            FocusManager.SetFocusedElement(this, null);
            var selectedRequest = collections.SelectedCollection.SelectedRequest;
            var fileNameWithoutExtension = selectedRequest.Name;
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                fileNameWithoutExtension = fileNameWithoutExtension.Replace(c, '-');
            }
            var dialog = new SaveFileDialog()
            {
                FileName = fileNameWithoutExtension + ".pwr",
                Filter = "Postwoman Requests (*.pwr)|*.pwr|All Files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == true)
            {
                var json = JsonConvert.SerializeObject(selectedRequest, serializationSettings);
                File.WriteAllText(dialog.FileName, json);
            }
        }

        private void ImportPostmanCollectionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "Postman Collections (*.json)|*.json|All Files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == true)
            {
                foreach (var collection in PostmanImporter.Import(dialog.FileName))
                {
                    collections.Collections.Add(collection);
                }
                collections.SelectedCollection = null;
            }
        }

        private void SaveCollectionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            FocusManager.SetFocusedElement(this, null);
            var fileNameWithoutExtension = collections.SelectedCollection.Name;
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                fileNameWithoutExtension = fileNameWithoutExtension.Replace(c, '-');
            }
            var dialog = new SaveFileDialog()
            {
                FileName = fileNameWithoutExtension + ".pwc",
                Filter = "Postwoman Collections (*.pwc)|*.pwc|All Files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == true)
            {
                var json = JsonConvert.SerializeObject(collections.SelectedCollection, serializationSettings);
                File.WriteAllText(dialog.FileName, json);
            }
        }

        private void LoadCollectionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "Postwoman Collections (*.pwc)|*.pwc|All Files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == true)
            {
                var text = File.ReadAllText(dialog.FileName);
                var collection = JsonConvert.DeserializeObject<CollectionViewModel>(text, serializationSettings);
                foreach (var group in collection.VariableGroups)
                {
                    if (!string.IsNullOrEmpty(group.InheritsGroupName))
                    {
                        group.Inherits = collection.VariableGroups.First(g => g.Name == group.InheritsGroupName);
                    }
                }
                foreach (var environment in collection.Environments)
                {
                    if (!string.IsNullOrEmpty(environment.ServerName))
                    {
                        environment.Server = collection.Servers.First(s => s.Name == environment.ServerName);
                    }
                    if (!string.IsNullOrEmpty(environment.VariableGroupName))
                    {
                        environment.VariableGroup = collection.VariableGroups.First(g => g.Name == environment.VariableGroupName);
                    }
                }

                // Upgrade older collections which only have a single variable group
                if (collection.Variables?.Count > 0)
                {
                    var newVariableGroup = new VariableGroupViewModel { Name = "Variables", Variables = collection.Variables };
                    collection.VariableGroups.Add(newVariableGroup);
                    if (collection.Servers.Count > 0)
                    {
                        collection.Environments.Add(new EnvironmentViewModel { Name = "Main", Server = collection.Servers.First(), VariableGroup = newVariableGroup });
                    }
                    collection.Variables = null;
                }
                collections.Collections.Add(collection);
                collections.SelectedCollection = collection;
            }
        }

        private void NewCollectionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var window = new NewCollectionWindow();
            if (window.ShowDialog() == true)
            {
                var newCollection = new CollectionViewModel { Name = window.CollectionName };
                collections.Collections.Add(newCollection);
                collections.SelectedCollection = newCollection;
            }
        }

        private void RenameCollectionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var window = new EditCollectionWindow(collections.SelectedCollection);
            window.ShowDialog();
        }

        private void DeleteCollectionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you wish to delete the selected collection?", "Delete Collection", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                collections.Collections.Remove(collections.SelectedCollection);
                collections.SelectedCollection = collections.Collections.FirstOrDefault();
            }
        }

        private void NewRequestMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var newRequest = new RequestViewModel { Name = "New request" };
            collections.SelectedCollection.Requests.Add(newRequest);
            collections.SelectedCollection.SelectedRequest = newRequest;
        }

        private void DeleteRequestMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you wish to delete the selected request?", "Delete Request", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                collections.SelectedCollection.Requests.Remove(collections.SelectedCollection.SelectedRequest);
                collections.SelectedCollection.SelectedRequest = null;
            }
        }

        private void DuplicateRequestMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedRequest = collections.SelectedCollection.SelectedRequest;
            var newRequest = selectedRequest.Clone("Copy of " + selectedRequest.Name);
            collections.SelectedCollection.Requests.Add(newRequest);
            collections.SelectedCollection.SelectedRequest = newRequest;
        }

        private void ExperimentWithRequestMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = DataContext as CollectionsViewModel;
            var newDataContext = new CollectionsViewModel
            {
                Collections = dataContext.Collections,
                SelectedCollection = dataContext.SelectedCollection
            };
            var selectedRequest = newDataContext.SelectedCollection.SelectedRequest;
            newDataContext.SelectedCollection.SelectedRequest = selectedRequest.Clone(selectedRequest.Name); // TODO: Fix cloned request being selected after window closed
            var window = new RequestResponseWindow(newDataContext);
            window.ShowDialog();
        }

        private void SortCollectionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            collections.SelectedCollection.Requests = new ObservableCollection<RequestViewModel>(
                collections.SelectedCollection.Requests.OrderBy(r => r.Name).ThenBy(r => r.Method)
            );
        }

        private void EditCollectionConfigurationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var window = new CollectionConfigurationWindow(collections.SelectedCollection);
            window.ShowDialog();
        }

        private void GenerateCodeForCollectionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var window = new GenerateCollectionCodeWindow(collections.SelectedCollection);
            window.ShowDialog();
        }

        private void GenerateCodeForRequestMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var window = new GenerateRequestCodeWindow(collections.SelectedCollection, collections.SelectedCollection.SelectedRequest);
            window.ShowDialog();
        }

    }

}
