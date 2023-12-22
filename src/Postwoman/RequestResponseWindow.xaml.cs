using Postwoman.Models.PwRequest;
using System.Windows;

namespace Postwoman
{

    /// <summary>
    /// Interaction logic for RequestResponseWindow.xaml
    /// </summary>
    public partial class RequestResponseWindow : Window
    {

        public RequestResponseWindow()
        {
            InitializeComponent();
        }

        public RequestResponseWindow(CollectionsViewModel dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
        }

    }

}
