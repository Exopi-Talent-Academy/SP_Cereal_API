using Cereal_API.Shared;
using Cereal_API.Shared.Extensions;
using Cereal_API.Shared.Models;
using System.Globalization;

namespace Cereal_API.API.Database;

public static class CsvImporter
{
    public static List<CerealData> Load(IFormFile file, List<ErrorReport>? errorReports = null)
    {
        List<CerealData> cereals = [];
        using var reader = new StreamReader(file.OpenReadStream());

        // Skip:
        // Row 0 = column names
        // Row 1 = data types
        reader.ReadLine();
        reader.ReadLine();
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            try
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                cereals.Add(ImportLine(line));
            }
            catch (ArgumentException ex)
            {
                DebugHelper.ReportException($"Validation failed for row:\n{line}", ex);
                errorReports?.Add(new ErrorReport($"Validation failed for row:\n{line}", ex.Message));
            }
            catch (Exception ex)
            {
                DebugHelper.ReportException($"Failed to import row:\n{line}", ex);
                errorReports?.Add(new ErrorReport($"Failed to import row:\n{line}", ex.Message));
            }
        }

        return cereals;
    }

    public static List<CerealData> Load(string path)
    {
        List<CerealData> cereals = [];
        var lines = File.ReadAllLines(path);

        // Skip:
        // Row 0 = column names
        // Row 1 = data types
        foreach (var line in lines.Skip(2))
        {
            try
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                cereals.Add(ImportLine(line));
            }
            catch (Exception ex)
            {
                DebugHelper.ReportException($"Failed to import row:\n{line}", ex);
            }
        }

        return cereals;
    }

    private static CerealData ImportLine(string line)
    {
        var values = line.Split(';');
        CerealData cereal = new()
        {
            Name = values[0],
            Manufacturer = Enum.Parse<CerealEnums.Manufacturer>(values[1]),
            Type = Enum.Parse<CerealEnums.Type>(values[2]),
            Calories = int.Parse(values[3]),
            Protein = int.Parse(values[4]),
            Fat = int.Parse(values[5]),
            Sodium = int.Parse(values[6]),
            Fiber = float.Parse(values[7], CultureInfo.InvariantCulture),
            Carbo = float.Parse(values[8], CultureInfo.InvariantCulture),
            Sugars = int.Parse(values[9]),
            Potass = int.Parse(values[10]),
            Vitamins = int.Parse(values[11]),
            Shelf = int.Parse(values[12]),
            Weight = float.Parse(values[13], CultureInfo.InvariantCulture),
            Cups = float.Parse(values[14], CultureInfo.InvariantCulture),
            Rating = float.Parse(values[15], CultureInfo.InvariantCulture),
            DbVersion = DateTime.UtcNow,
        };
        cereal.Validate();
        
        return cereal;
    }
}
