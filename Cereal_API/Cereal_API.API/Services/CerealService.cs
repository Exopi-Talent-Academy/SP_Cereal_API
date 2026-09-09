using Cereal_API.API.Database;
using Cereal_API.Shared.DTOs;
using Cereal_API.Shared.Extensions;
using Cereal_API.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Cereal_API.API.Services;

public class CerealService(AppDbContext db, IHttpContextAccessor httpContextAccessor, string imageFolder = "") : ICerealService
{
    private readonly AppDbContext _db = db;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private string CurrentUsername => _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "Unknown User";
    private readonly string _imageFolder = imageFolder;
    private readonly string[] imageExtensions = [".jpg", ".jpeg", ".png"];

    public List<CerealData> GetAllCereals()
    {
        return _db.Cereals
            .Include(c => c.ChangeHistory)
            .ToList();
    }

    public CerealData GetCereal(int id)
    {
        return _db.Cereals
            .Include(c => c.ChangeHistory)
            .FirstOrDefault(c => c.Id == id) 
            ?? throw new KeyNotFoundException($"Cereal with id '{id}' not found");
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

    public CerealData CreateCereal(CreateCerealDTO cerealDTO)
    {
        CerealData cereal = cerealDTO.ToCerealData();

        cereal.Validate();

        cereal.Id = 0;
        cereal.DbVersion = DateTime.UtcNow;
        cereal.AddCreatedChangeHistory(CurrentUsername);
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
            cereal.AddCreatedChangeHistory(CurrentUsername);
            _db.Cereals.Add(cereal);
        }
        _db.SaveChanges();
    }

    public CerealData UpdateCereal(UpdateCerealDTO cereal)
    {
        CerealData dbCereal = GetCereal(cereal.Id);
        
        if (dbCereal.DbVersion != cereal.DbVersion)
            throw new InvalidOperationException();

        if (dbCereal.IsEqual(cereal))
            return dbCereal;

        cereal.DbVersion = DateTime.UtcNow;
        dbCereal.AddUpdatedChangeHistory(cereal, CurrentUsername);
        _db.Cereals.Entry(dbCereal).CurrentValues.SetValues(cereal);
        _db.SaveChanges();

        return dbCereal;
    }

    public CerealData DeleteCereal(int id)
    {
        CerealData dbCereal = GetCereal(id);

        dbCereal.MarkedAsDeleted = true;
        dbCereal.DbVersion = DateTime.UtcNow;
        dbCereal.AddDeletedChangeHistory(CurrentUsername);
        _db.SaveChanges();

        return dbCereal;
    }

    public CerealData RestoreCereal(int id)
    {
        CerealData dbCereal = GetCereal(id);

        dbCereal.MarkedAsDeleted = false;
        dbCereal.DbVersion = DateTime.UtcNow;
        dbCereal.AddRestoredChangeHistory(CurrentUsername);
        _db.SaveChanges();

        return dbCereal;
    }
}
