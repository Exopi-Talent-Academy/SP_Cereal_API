using Cereal_API.API.Database;
using Cereal_API.Shared.Extensions;
using Cereal_API.Shared.Models;

namespace Cereal_API.API.Services;

public class CerealService(AppDbContext db, string imageFolder = "") : ICerealService
{
    private readonly AppDbContext _db = db;
    private readonly string _imageFolder = imageFolder;
    private readonly string[] imageExtensions = [".jpg", ".jpeg", ".png"];

    public List<CerealData> GetAllCereals()
    {
        return _db.Cereals.ToList();
    }

    public CerealData GetCereal(int id)
    {
        return _db.Cereals.FirstOrDefault(c => c.Id == id) ?? throw new KeyNotFoundException($"Cereal with id {id} not found");
    }

    public (byte[] Data, string ContentType) GetImage(int id)
    {
        CerealData existingCereal = GetCereal(id);

        string? imagePath = imageExtensions
            .Select(extension => Path.Combine(_imageFolder, existingCereal.Name + extension))
            .FirstOrDefault(File.Exists);

        if (imagePath == null) 
            throw new FileNotFoundException($"Cereal image \"{existingCereal.Name}\" with id {id} not found at path: {imagePath}", imagePath);

        string contentType = Path.GetExtension(imagePath).ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            _ => "application/octet-stream"
        };

        return (File.ReadAllBytes(imagePath), contentType);
    }

    public void CreateCereal(CerealData cereal)
    {
        if (!Enum.IsDefined(cereal.Manufacturer)) throw new ArgumentException("Invalid manufacturer");
        if (!Enum.IsDefined(cereal.Type)) throw new ArgumentException("Invalid cereal type");

        cereal.Id = 0;
        cereal.DbVersion = DateTime.UtcNow;
        _db.Cereals.Add(cereal);
        _db.SaveChanges();
    }

    public void UpdateCereal(CerealData cereal)
    {
        CerealData existingCereal = GetCereal(cereal.Id);
        
        if (existingCereal.DbVersion != cereal.DbVersion)
            throw new InvalidOperationException();

        if (existingCereal.IsEqual(cereal))
            return;

        cereal.DbVersion = DateTime.UtcNow;
        _db.Cereals.Entry(existingCereal).CurrentValues.SetValues(cereal);
        _db.SaveChanges();
    }

    public void DeleteCereal(int id)
    {
        CerealData existingCereal = GetCereal(id);

        _db.Cereals.Remove(existingCereal);
        _db.SaveChanges();
    }
}
