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

    public CerealData CreateCereal(CerealData cereal)
    {
        cereal.Validate();

        cereal.Id = 0;
        cereal.DbVersion = DateTime.UtcNow;
        _db.Cereals.Add(cereal);
        _db.SaveChanges();
        
        return cereal;
    }

    public void CreateCereals(List<CerealData> cereals)
    {
        foreach (CerealData cereal in cereals)
        {
            cereal.Validate();

            cereal.Id = 0;
            cereal.DbVersion = DateTime.UtcNow;
            _db.Cereals.Add(cereal);
        }
        _db.SaveChanges();
    }

    public CerealData UpdateCereal(CerealData cereal)
    {
        CerealData dbCereal = GetCereal(cereal.Id);
        
        if (dbCereal.DbVersion != cereal.DbVersion)
            throw new InvalidOperationException();

        if (dbCereal.IsEqual(cereal))
            return dbCereal;

        cereal.DbVersion = DateTime.UtcNow;
        _db.Cereals.Entry(dbCereal).CurrentValues.SetValues(cereal);
        _db.SaveChanges();

        return dbCereal;
    }

    public CerealData DeleteCereal(int id)
    {
        CerealData dbCereal = GetCereal(id);

        dbCereal.MarkedAsDeleted = true;
        dbCereal.DbVersion = DateTime.UtcNow;
        _db.Cereals.Update(dbCereal);
        _db.SaveChanges();

        return dbCereal;
    }

    public CerealData RestoreCereal(int id)
    {
        CerealData dbCereal = GetCereal(id);

        dbCereal.MarkedAsDeleted = false;
        dbCereal.DbVersion = DateTime.UtcNow;
        _db.Cereals.Update(dbCereal);
        _db.SaveChanges();

        return dbCereal;
    }
}
