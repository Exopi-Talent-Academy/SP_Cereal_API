using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Cereal_API.Shared;

public static class DebugHelper
{
    public static void Report(string message, [CallerMemberName] string? methodName = null)
    {
        Debug.WriteLine($"{methodName} Debug: {message}");
    }

    public static void ReportException(Exception ex, [CallerMemberName] string? methodName = null)
    {
        Report($"{methodName} threw the error: {ex.Message}");
    }

    public static void ReportException(string message, Exception ex, [CallerMemberName] string? methodName = null)
    {
        Report($"{methodName} threw the error: {message}\n{ex.Message}");
    }
}
