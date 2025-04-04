using Postwoman.Models.PwRequestViewModel;
using System;
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
