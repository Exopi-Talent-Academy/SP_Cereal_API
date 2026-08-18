using Cereal_API.WPF.Services;
using System.Windows;

namespace Cereal_API.WPF;

public partial class MainWindow : Window
{
    private readonly CerealApiService _api;

    public MainWindow(CerealApiService api)
    {
        InitializeComponent();

        _api = api;
    }
}