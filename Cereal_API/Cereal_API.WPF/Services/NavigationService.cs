using Cereal_API.WPF.ViewModels;

namespace Cereal_API.WPF.Services;

public class NavigationService : INavigationService
{
    private ViewModelBase? _current;
    public ViewModelBase? CurrentViewModel => _current;

    public event Action? CurrentViewChanged;

    public void Navigate(ViewModelBase vm)
    {
        _current = vm;
        CurrentViewChanged?.Invoke();
    }
}
