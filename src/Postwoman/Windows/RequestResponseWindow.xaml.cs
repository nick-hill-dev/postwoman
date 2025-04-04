using Postwoman.Models.PwRequestViewModel;
using System.Windows;

namespace Postwoman.Windows
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
