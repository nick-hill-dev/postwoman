using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Postwoman.Models.PwRequest;
using Postwoman.Models.PwRequestViewModel;
using System;
using System.Linq;
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
                var requestMaker = new RequestMaker(selectedCollection);
                var response = await requestMaker.Send(selectedRequest);
                selectedRequest.LatestResponse = response;

                foreach (var action in (selectedRequest.Actions ?? []).Where(a => a.When == "AfterResponse"))
                {
                    switch (action.Action)
                    {
                        case "SetVariable":
                            var variableGroup = (selectedCollection?.SelectedEnvironment?.VariableGroup) ?? throw new Exception("No environment selected.");
                            var variable = variableGroup.Find(action.VariableName) ?? throw new Exception($"Variable '{action.VariableName}' not found.");
                            var jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Body);
                            variable.Value = jsonResponse[action.PropertyName]?.ToString();
                            break;

                        default:
                            throw new Exception($"Unknown action: {action.Action}");
                    }
                }

                var fullUrl = UrlTools.GetFullUrl(selectedCollection, selectedRequest);
                StatusTextBlock.Text = $"HTTP {selectedRequest.Method} {fullUrl} ({response.StatusCode})";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }

}
