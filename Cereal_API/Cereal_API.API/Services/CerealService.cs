using Cereal_API.API.Database;
using Cereal_API.Shared.Models;

namespace Cereal_API.API.Services;

public class CerealService(AppDbContext db, string imageFolder = "") : ICerealService
{
    private readonly AppDbContext _db = db;
    private readonly string _imageFolder = imageFolder;

    public List<CerealData> GetAllCereals()
    {
        return _db.Cereals.ToList();
    }

    public CerealData GetCereal(int id)
    {
        return _db.Cereals.FirstOrDefault(c => c.Id == id) ?? throw new KeyNotFoundException($"Cereal with id {id} not found");
    }

    public void GetImage(int id)    // Unsure about the return type here
    {
        throw new NotImplementedException();
    }

    public void CreateCereal(CerealData cereal)
    {
        cereal.Id = 0;
        _db.Cereals.Add(cereal);
        _db.SaveChanges();
    }

    public void UpdateCereal(CerealData cereal)
    {
        throw new NotImplementedException();
    }

    public void DeleteCereal(int id)
    {
        throw new NotImplementedException();
    }
}
