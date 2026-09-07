namespace Cereal_API.Tests.Helper;

public static class MockFilesHelper
{
    public static string MockFolderPath => Path.Combine(AppContext.BaseDirectory, "Mock Files");

    public static string GetMockFilePath(string fileName)
    {
        return Path.Combine(MockFolderPath, fileName);
    }
}
