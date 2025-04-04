using Postwoman.CodeGeneration;
using Postwoman.CodeGeneration.PowerShell;
using Postwoman.CodeGeneration.Swagger;
using Postwoman.Models.PwRequestViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Postwoman.Windows;

/// <summary>
/// Interaction logic for GenerateRequestCodeWindow.xaml
/// </summary>
public partial class GenerateRequestCodeWindow : Window
{

    private readonly CollectionViewModel _collection;

    private readonly RequestViewModel _request;

    public GenerateRequestCodeWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        ScenarioComboBox_SelectionChanged(null, null);
    }

    public GenerateRequestCodeWindow(CollectionViewModel collection, RequestViewModel request)
    {
        InitializeComponent();
        _collection = collection;
        _request = request;
    }

    private void ScenarioComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var item = (ScenarioComboBox.SelectedItem as ComboBoxItem).Content?.ToString();
        if (item != null)
        {
            var generator = (ICodeGenerator)null;
            switch (item)
            {
                case "PowerShell":
                    generator = new PowerShellCodeGenerator();
                    break;

                case "Swagger":
                    generator = new SwaggerCodeGenerator();
                    break;
            }
            var code = generator.Generate(_collection, _request);
            CodeTextBox.Text = code;
        }
    }

}
