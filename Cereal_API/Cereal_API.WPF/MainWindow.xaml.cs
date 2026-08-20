using Cereal_API.WPF.Services;
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
}