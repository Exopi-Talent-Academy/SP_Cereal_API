using Cereal_API.WPF.Commands;
using Cereal_API.WPF.Helpers;
using Cereal_API.WPF.Services;
using System.Windows.Controls;

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

    public bool IsLoggedIn => _authenticationState.IsLoggedIn;
    public string LoggedInUsername => $"{_authenticationState.Username ?? string.Empty} ({_authenticationState.Role ?? string.Empty})";

    public RelayCommand LoginCommand { get; set; }

    public RelayCommand LogoutCommand { get; set; }

    public LoginViewModel(CerealHttpService api, AuthenticationState authenticationState)
    {
        _api = api;
        _authenticationState = authenticationState;

        LoginCommand = new RelayCommand(
            execute: param => _ = Login(param),
            canExecute: _ => true);

        LogoutCommand = new RelayCommand(
            execute: param => _ = Logout(param),
            canExecute: _ => true);
    }

    public async Task Login(object? parameter)
    {
        string password = parameter switch
        {
            PasswordBox pb => pb.Password,
            string s => s,
            _ => string.Empty
        };

        var loginResult = await _api.Login(Username, password);
        if (loginResult != null)
        {
            _authenticationState.SetLoggedIn(loginResult.Username, loginResult.Role);
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(LoggedInUsername));
        }
        else
        {
            MessageBoxHelper.ShowError("Invalid Username or Password!", "Failed!");
        }
    }

    public async Task Logout(object? parameter)
    {
        await _api.Logout();

        Username = string.Empty;
        if (parameter is PasswordBox passwordBox)
        {
            passwordBox.Password = string.Empty;
        }

        _authenticationState.SetLoggedOut();
        OnPropertyChanged(nameof(IsLoggedIn));
    }
}
