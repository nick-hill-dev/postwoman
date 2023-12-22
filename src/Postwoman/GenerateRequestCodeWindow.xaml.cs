using Postwoman.CodeGeneration.Swagger;
using Postwoman.Models.PwRequest;
using System.Windows;

namespace Postwoman;

/// <summary>
/// Interaction logic for GenerateRequestCodeWindow.xaml
/// </summary>
public partial class GenerateRequestCodeWindow : Window
{

    public GenerateRequestCodeWindow()
    {
        InitializeComponent();
    }

    public GenerateRequestCodeWindow(CollectionViewModel collection, RequestViewModel request)
    {
        InitializeComponent();
        var generator = new SwaggerCodeGenerator();
        var code = generator.Generate(collection, request);
        CodeTextBox.Text = code;
    }

}
