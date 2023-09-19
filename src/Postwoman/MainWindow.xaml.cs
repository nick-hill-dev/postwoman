using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Postwoman.Models.PostmanCollection;
using Postwoman.Models.PwRequest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
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
            collections.Collections.Add(new CollectionViewModel
            {
                Name = "My Collection",
                Requests = new ObservableCollection<RequestViewModel>()
                {
                    new RequestViewModel
                    {
                        Name = "New request"
                    }
                }
            });
            DataContext = collections;
        }

        private async void SendRequestButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedCollection = collections.SelectedCollection;
                var selectedRequest = selectedCollection.SelectedRequest;
                var requestMethod = GetRequestMethod(selectedRequest.Method);
                var fullUrl = VariableReplacer.Replace(selectedRequest.Url, selectedCollection.Variables);
                var request = new HttpRequestMessage(requestMethod, fullUrl);

                foreach (var header in selectedRequest.Headers)
                {
                    request.Headers.TryAddWithoutValidation(
                        header.Name,
                        VariableReplacer.Replace(header.Value, selectedCollection.Variables)
                    );
                }

                if (!string.IsNullOrEmpty(selectedRequest.Body))
                {
                    var mediaType = "application/json";
                    request.Content = new StringContent(
                        VariableReplacer.Replace(selectedRequest.Body, selectedCollection.Variables),
                        Encoding.UTF8,
                        mediaType
                    );
                }

                var client = new HttpClient();
                var response = await client.SendAsync(request);
                var responseText = await response.Content.ReadAsStringAsync();

                selectedRequest.LatestResponse = new ResponseViewModel
                {
                    Body = responseText,
                    Headers = new ObservableCollection<ResponseHeaderViewModel>(response.Content.Headers.Select(h => new ResponseHeaderViewModel
                    {
                        Name = h.Key,
                        Value = string.Join(", ", h.Value)
                    }))
                };

                StatusTextBlock.Text = $"HTTP {selectedRequest.Method} {fullUrl} ({response.StatusCode})";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static HttpMethod GetRequestMethod(string method)
        {
            var requestMethod = HttpMethod.Get;
            switch (method)
            {
                case "POST":
                    requestMethod = HttpMethod.Post;
                    break;

                case "PUT":
                    requestMethod = HttpMethod.Put;
                    break;

                case "DELETE":
                    requestMethod = HttpMethod.Delete;
                    break;

                case "OPTIONS":
                    requestMethod = HttpMethod.Options;
                    break;
            }

            return requestMethod;
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
                var text = File.ReadAllText(dialog.FileName);
                var postmanCollection = JsonConvert.DeserializeObject<ExportedPostmanCollections>(text, serializationSettings);
                foreach (var collection in postmanCollection.Collections.OrderBy(c => c.Name))
                {
                    var newCollection = new CollectionViewModel
                    {
                        Name = collection.Name
                    };
                    foreach (var variable in collection.Variables)
                    {
                        newCollection.Variables.Add(new VariableViewModel
                        {
                            Name = variable.Key,
                            Value = variable.Value
                        });
                    }
                    foreach (var request in collection.Requests)
                    {
                        var newRequest = new RequestViewModel
                        {
                            Method = request.Method,
                            Name = request.Name,
                            Url = request.Url
                        };
                        foreach (var header in request.HeaderData ?? new List<PostmanHeaderDataItem>())
                        {
                            newRequest.Headers.Add(new RequestHeaderViewModel
                            {
                                Name = header.Key,
                                Value = header.Value
                            });
                        }
                        if (!string.IsNullOrEmpty(request.RawModeData))
                        {
                            newRequest.Body = request.RawModeData;
                        }
                        newCollection.Requests.Add(newRequest);
                    }
                    collections.Collections.Add(newCollection);
                }
                collections.SelectedCollection = null;
            }
        }

        private void NewCollectionButton_Click(object sender, RoutedEventArgs e)
        {
            var newCollection = new CollectionViewModel { Name = "New collection" };
            collections.Collections.Add(newCollection);
            collections.SelectedCollection = newCollection;
        }

        private void NewRequestButton_Click(object sender, RoutedEventArgs e)
        {
            var newRequest = new RequestViewModel { Name = "New request" };
            collections.SelectedCollection.Requests.Add(newRequest);
            collections.SelectedCollection.SelectedRequest = newRequest;
        }

        private void EditCollectionVariablesButton_Click(object sender, RoutedEventArgs e)
        {
            new CollectionVariablesWindow(collections.SelectedCollection).ShowDialog();
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
                collections.Collections.Add(collection);
                collections.SelectedCollection = collection;
            }
        }

    }

}
