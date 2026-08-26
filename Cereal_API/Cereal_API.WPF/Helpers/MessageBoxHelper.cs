using System.Runtime.CompilerServices;
using System.Windows;

namespace Cereal_API.WPF.Helpers;

public static class MessageBoxHelper
{
    public static void ShowError(string errorMessage, string errorCaption = "Error!", [CallerMemberName] string? methodName = null)
    {
        MessageBox.Show($"{methodName} threw the following error: {errorMessage}", $"{methodName} {errorCaption}", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
