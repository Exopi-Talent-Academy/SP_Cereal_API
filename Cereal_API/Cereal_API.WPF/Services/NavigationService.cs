using Cereal_API.WPF.ViewModels;

namespace Cereal_API.WPF.Services;

public class NavigationService(MainViewModel mainViewModel) : INavigationService
{
    private readonly MainViewModel _mainViewModel = mainViewModel;

    public void Navigate(ViewModelBase vm)
    {
        _mainViewModel.CurrentViewModel = vm;
    }
}
