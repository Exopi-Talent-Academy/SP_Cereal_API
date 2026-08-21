using System.Diagnostics;

namespace Cereal_API.Shared;

public static class DebugHelper
{
    public static void Report(string message)
    {
        Debug.WriteLine(message);
    }

    public static void ReportException(string methodName, Exception ex)
    {
        Report($"{methodName} threw the error: {ex.Message}");
    }
}
