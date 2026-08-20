using Cereal_API.API.Database;
using Microsoft.EntityFrameworkCore;

namespace Cereal_API.Tests.Helper;

public static class FakeDbContext
{
    public static AppDbContext Create()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        return new AppDbContext(options);
    }

    public static string ImageFolderPath => Path.Combine(AppContext.BaseDirectory, "APITests", "Images");
}
