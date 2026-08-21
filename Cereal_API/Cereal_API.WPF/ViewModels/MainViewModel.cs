using Cereal_API.WPF.Services;

namespace Cereal_API.WPF.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly INavigationService _navigation;

    public ViewModelBase? CurrentViewModel => _navigation.CurrentViewModel;

    public MainViewModel(INavigationService navigation, CerealDataTableViewModel startViewModel)
    {
        _navigation = navigation;

        _navigation.CurrentViewChanged += () =>
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        };

        _navigation.Navigate(startViewModel);
    }
}
