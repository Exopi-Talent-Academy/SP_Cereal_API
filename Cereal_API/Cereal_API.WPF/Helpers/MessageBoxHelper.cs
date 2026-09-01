using System.Runtime.CompilerServices;
using System.Windows;

namespace Cereal_API.WPF.Helpers;

public static class MessageBoxHelper
{
    private const string AreYouSure = "\n\nAre you sure?";

    public static bool Confirm(string message, string caption = "Confirm")
    {
        return MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
    }

    public static bool ConfirmLoadCereals()
    {
        return Confirm($"You about to load all cereals, discarding any current changes. {AreYouSure}", $"Confirm Load Cereals");
    }

    public static bool ConfirmSaveNewCereal()
    {
        return Confirm($"You about to save the new cereal to the database! {AreYouSure}", $"Confirm Save New Cereal");
    }

    public static bool ConfirmCancelNewCereal()
    {
        return Confirm($"You about to cancel and delete the new cereal. {AreYouSure}", $"Confirm Cancel New Cereal");
    }

    public static bool ConfirmUpdateChanges(string subject)
    {
        return Confirm($"You about to update the changes for \"{subject}\" to the database! {AreYouSure}", $"Confirm Update Changes");
    }

    public static bool ConfirmCancelChanges(string subject)
    {
        return Confirm($"You about to cancel the changes to \"{subject}\". {AreYouSure}", $"Confirm Cancel Changes");
    }

    public static bool ConfirmDelete(string subject)
    {
        return Confirm($"You about to delete \"{subject}\" from the database! {AreYouSure}", $"Confirm Delete");
    }

    public static void ShowError(string errorMessage, string errorCaption = "Error!", [CallerMemberName] string? methodName = null)
    {
        MessageBox.Show($"{methodName} threw the following error: {errorMessage}", $"{methodName} {errorCaption}", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
