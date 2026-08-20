using Cereal_API.WPF.Services;
using System.Diagnostics;
using System.Windows;

namespace Cereal_API.WPF;

public partial class MainWindow : Window
{
    private readonly CerealHttpService _api;

    public MainWindow(CerealHttpService api)
    {
        InitializeComponent();

        _api = api;
    }

    private void GetAllCerealsButton_Click(object sender, RoutedEventArgs e)
    {
        // Still doesn't work!
        var result = _api.GetAllCereals();
        Trace.WriteLine(result.Result.Count);
    }
}