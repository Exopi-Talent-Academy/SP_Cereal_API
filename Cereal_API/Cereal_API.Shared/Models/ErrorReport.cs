namespace Cereal_API.Shared.Models;

public class ErrorReport(string message, string error)
{
    public string Message { get; set; } = message;
    public string Error { get; set; } = error;
}
