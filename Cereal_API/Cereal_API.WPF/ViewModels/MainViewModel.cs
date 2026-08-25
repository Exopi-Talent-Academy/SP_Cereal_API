using Cereal_API.WPF.Services;

namespace Cereal_API.WPF.ViewModels;

public class MainViewModel(LoginViewModel loginVM, CerealDataTableViewModel cerealDataTableVM) : ViewModelBase
{
    public LoginViewModel LoginVM { get; } = loginVM;

    public CerealDataTableViewModel CerealDataTableVM { get; } = cerealDataTableVM;
}
