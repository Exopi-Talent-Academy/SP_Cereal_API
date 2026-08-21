using Cereal_API.WPF.ViewModels;

namespace Cereal_API.WPF.Services;

public interface INavigationService
{
    void Navigate(ViewModelBase vm);
}
