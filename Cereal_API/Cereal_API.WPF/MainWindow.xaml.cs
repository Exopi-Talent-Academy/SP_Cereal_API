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

    private async void GetAllCerealsButton_Click(object sender, RoutedEventArgs e)
    {
        var result = await _api.GetAllCereals();
        Trace.WriteLine(result.Count);
    }
}