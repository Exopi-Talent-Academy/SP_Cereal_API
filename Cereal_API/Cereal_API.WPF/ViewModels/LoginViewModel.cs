using Cereal_API.WPF.Commands;
using Cereal_API.WPF.Services;
using System.Windows;

namespace Cereal_API.WPF.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly CerealHttpService _api;

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

    private bool _isLoggedIn;
    public bool IsLoggedIn
    {
        get => _isLoggedIn;
        private set
        {
            if (_isLoggedIn == value)
                return;

            _isLoggedIn = value;
            OnPropertyChanged();
        }
    }

    public RelayCommand LoginCommand { get; set; }

    public RelayCommand LogoutCommand { get; set; }

    public LoginViewModel(CerealHttpService api)
    {
        _api = api;

        LoginCommand = new RelayCommand(
            execute: _ => _ = Login(),
            canExecute: _ => true);

        LogoutCommand = new RelayCommand(
            execute: _ => _ = Logout(),
            canExecute: _ => true);
    }

    public async Task Login()
    {
        IsLoggedIn = await _api.Login(Username, Password);

        if (!IsLoggedIn)
        {
            MessageBox.Show("Invalid Username or Password!", "Login failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public async Task Logout()
    {
        await _api.Logout();

        Username = string.Empty;
        Password = string.Empty;
        IsLoggedIn = false;
    }
}
