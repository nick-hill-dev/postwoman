using System.Windows;

namespace Postwoman;

/// <summary>
/// Interaction logic for GeneratedCodeWindow.xaml
/// </summary>
public partial class GeneratedCodeWindow : Window
{

    public GeneratedCodeWindow()
    {
        InitializeComponent();
    }

    public GeneratedCodeWindow(string code)
    {
        InitializeComponent();
        CodeTextBox.Text = code;
    }

}
