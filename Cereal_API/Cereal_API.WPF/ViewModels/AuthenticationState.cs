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

    public void SetLoggedIn(bool loggedIn)
    {
        IsLoggedIn = loggedIn;
    }
}
