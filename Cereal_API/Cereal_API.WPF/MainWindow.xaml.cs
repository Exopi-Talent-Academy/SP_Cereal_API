using Cereal_API.WPF.ViewModels;
using System.Windows;

namespace Cereal_API.WPF;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel vm)
    {
        InitializeComponent();

        DataContext = vm;
    }
}