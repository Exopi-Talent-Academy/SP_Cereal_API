using Cereal_API.Shared;
using Cereal_API.WPF.Commands;
using Cereal_API.WPF.Services;
using System.Windows;

namespace Cereal_API.WPF.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly CerealHttpService _api;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public bool IsLoggedIn { get; private set; } = false;

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
        DebugHelper.Report("Trying to login!");

        IsLoggedIn = await _api.Login(Username, Password);

        DebugHelper.Report($"InLoggedIn = {IsLoggedIn}");

        if (!IsLoggedIn)
        {
            MessageBox.Show("Invalid Username or Password!", "Login failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public async Task Logout()
    {
        DebugHelper.Report("Trying to logout!");

        await _api.Logout();
        IsLoggedIn = false;
    }
}
