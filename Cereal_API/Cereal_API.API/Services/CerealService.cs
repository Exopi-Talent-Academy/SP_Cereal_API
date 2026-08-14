using Cereal_API.API.Database;
using Cereal_API.Shared.Models;

namespace Cereal_API.API.Services;

public class CerealService(AppDbContext db) : ICerealService
{
    private readonly AppDbContext _db = db;

    public List<CerealData> GetAllCereals()
    {
        throw new NotImplementedException();
    }

    public CerealData GetCereal(int id)
    {
        //throw new NotImplementedException();
        return _db.Cereals.FirstOrDefault(c => c.Id == id) ?? throw new KeyNotFoundException($"Cereal with id {id} not found");
    }

    public void GetImage(int id)    // Unsure about the return type here
    {
        throw new NotImplementedException();
    }

    public void CreateCereal(CerealData cereal)
    {
        throw new NotImplementedException();
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
