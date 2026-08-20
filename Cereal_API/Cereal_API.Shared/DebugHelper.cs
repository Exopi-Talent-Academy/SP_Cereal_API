using System.Diagnostics;

namespace Cereal_API.Shared;

public static class DebugHelper
{
    public static void ReportException(string methodName, Exception ex)
    {
        string message = $"{methodName} threw the error: {ex.Message}";
        Debug.WriteLine(message);
        Trace.WriteLine(message);
    }
}
