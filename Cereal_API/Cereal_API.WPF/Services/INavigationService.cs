using Cereal_API.WPF.ViewModels;

namespace Cereal_API.WPF.Services;

public interface INavigationService
{
    ViewModelBase? CurrentViewModel { get; }

    void Navigate(ViewModelBase vm);

    event Action? CurrentViewChanged;
}
