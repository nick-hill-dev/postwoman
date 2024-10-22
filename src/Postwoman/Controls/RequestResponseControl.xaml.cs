using Newtonsoft.Json;
using Postwoman.Models.PwRequest;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Postwoman.Controls
{

    /// <summary>
    /// Interaction logic for RequestResponseControl.xaml
    /// </summary>
    public partial class RequestResponseControl : UserControl
    {

        public RequestResponseControl()
        {
            InitializeComponent();
        }

        private async void SendRequestButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var collections = DataContext as CollectionsViewModel;
                var selectedCollection = collections.SelectedCollection;
                var selectedRequest = selectedCollection.SelectedRequest;
                var requestMethod = GetRequestMethod(selectedRequest.Method);

                var fullUrl = UrlTools.GetFullUrl(selectedCollection, selectedRequest);

                var request = new HttpRequestMessage(requestMethod, fullUrl);

                var variables = VariableCompiler.Compile(selectedCollection.SelectedEnvironment?.VariableGroup ?? new());
                foreach (var header in selectedCollection.Headers)
                {
                    request.Headers.TryAddWithoutValidation(
                        header.Name,
                        VariableReplacer.Replace(header.Value, variables)
                    );
                }

                foreach (var header in selectedRequest.Headers)
                {
                    request.Headers.TryAddWithoutValidation(
                        header.Name,
                        VariableReplacer.Replace(header.Value, variables)
                    );
                }

                switch (selectedRequest.Authorization.Type)
                {
                    case "Basic":
                        var userName = VariableReplacer.Replace(selectedRequest.Authorization.BasicUserName, variables);
                        var password = VariableReplacer.Replace(selectedRequest.Authorization.BasicPassword, variables);
                        var authenticationString = $"{userName}:{password}";
                        var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));
                        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                        break;

                    case "ApiKey":
                        var apiKey = VariableReplacer.Replace(selectedRequest.Authorization.ApiKeyHeaderName, variables);
                        request.Headers.TryAddWithoutValidation(apiKey, selectedRequest.Authorization.ApiKeyValue);
                        break;

                    case "Bearer":
                        var bearerToken = VariableReplacer.Replace(selectedRequest.Authorization.BearerToken, variables);
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                        break;
                }

                if (!string.IsNullOrEmpty(selectedRequest.Body))
                {
                    var specifiedContentType = selectedRequest.Headers.FirstOrDefault(h => h.Name == "Content-Type")?.Value;
                    var mediaType = specifiedContentType ?? "application/json";
                    request.Content = new StringContent(
                        VariableReplacer.Replace(selectedRequest.Body, variables),
                        Encoding.UTF8,
                        mediaType
                    );
                }

                var client = new HttpClient();
                var response = await client.SendAsync(request);

                var responseText = await response.Content.ReadAsStringAsync();
                if (response.Content.Headers.ContentType.MediaType == "application/json")
                {
                    try
                    {
                        responseText = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(responseText), Formatting.Indented);
                    }
                    catch
                    {
                    }
                }
                selectedRequest.LatestResponse = new ResponseViewModel
                {
                    Body = responseText,
                    StatusCode = (int)response.StatusCode,
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

    }

}
