namespace Cereal_API.WPF.ViewModels;

public class AuthenticationState : ViewModelBase
{
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

    private string? _username;
    public string? Username
    {
        get => _username;
        private set
        {
            if (_username == value)
                return;

            _username = value;
            OnPropertyChanged();
        }
    }

    private string? _role;
    public string? Role
    {
        get => _role;
        private set
        {
            if (_role == value)
                return;

            _role = value;
            OnPropertyChanged();
        }
    }

    public bool RoleHasEditPermission => IsLoggedIn && Role is "Editor" or "Admin";
    public bool RoleHasDeletePermission => IsLoggedIn && Role is "Admin";

    public void SetLoggedIn(string username, string role)
    {
        Username = username;
        Role = role;
        IsLoggedIn = true;
    }

    public void SetLoggedOut()
    {
        Username = null;
        Role = null;
        IsLoggedIn = false;
    }
}
