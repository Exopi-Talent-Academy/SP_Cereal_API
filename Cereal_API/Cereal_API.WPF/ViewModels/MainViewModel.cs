namespace Cereal_API.WPF.ViewModels;

public class MainViewModel : ViewModelBase
{
    private ViewModelBase? _currentViewModel;
    public ViewModelBase? CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnPropertyChanged();
        }
    }

    public MainViewModel(CerealDataTableViewModel startViewModel)
    {
        CurrentViewModel = startViewModel;
    }

    public void Navigate(ViewModelBase vm)
    {
        CurrentViewModel = vm;
    }
}
