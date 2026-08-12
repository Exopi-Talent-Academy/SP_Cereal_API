using Microsoft.EntityFrameworkCore;

namespace Cereal_API.API.Database;

public static class SeedData
{
    public static async Task InitializeAsync(AppDbContext db)
    {
        if (await db.Cereals.AnyAsync()) return;

        string csvPath = Path.Combine(AppContext.BaseDirectory, "Database", "cereal.csv");
        var cereals = CsvImporter.Load(csvPath);
        db.Cereals.AddRange(cereals);

        await db.SaveChangesAsync();
    }
}
