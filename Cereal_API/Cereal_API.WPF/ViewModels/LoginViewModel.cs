using Cereal_API.WPF.Commands;
using Cereal_API.WPF.Helpers;
using Cereal_API.WPF.Services;

namespace Cereal_API.WPF.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly CerealHttpService _api;
    private readonly AuthenticationState _authenticationState;

    private string _username = string.Empty;
    public string Username
    {
        get => _username;
        set
        {
            if (_username == value)
                return;

            _username = value;
            OnPropertyChanged();
        }
    }

    private string _password = string.Empty;
    public string Password
    {
        get => _password;
        set
        {
            if (_password == value)
                return;

            _password = value;
            OnPropertyChanged();
        }
    }

    public bool IsLoggedIn => _authenticationState.IsLoggedIn;

    public RelayCommand LoginCommand { get; set; }

    public RelayCommand LogoutCommand { get; set; }

    public LoginViewModel(CerealHttpService api, AuthenticationState authenticationState)
    {
        _api = api;
        _authenticationState = authenticationState;

        LoginCommand = new RelayCommand(
            execute: _ => _ = Login(),
            canExecute: _ => true);

        LogoutCommand = new RelayCommand(
            execute: _ => _ = Logout(),
            canExecute: _ => true);
    }

    public async Task Login()
    {
        _authenticationState.SetLoggedIn(await _api.Login(Username, Password));
        OnPropertyChanged(nameof(IsLoggedIn));

        if (!_authenticationState.IsLoggedIn)
        {
            MessageBoxHelper.ShowError("Invalid Username or Password!", "Failed!");
        }
    }

    public async Task Logout()
    {
        await _api.Logout();

        Username = string.Empty;
        Password = string.Empty;
        _authenticationState.SetLoggedIn(false);
        OnPropertyChanged(nameof(IsLoggedIn));
    }
}
