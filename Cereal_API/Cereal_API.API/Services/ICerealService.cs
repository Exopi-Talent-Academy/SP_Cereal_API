using Cereal_API.Shared.Models;

namespace Cereal_API.API.Services;

public interface ICerealService
{
    List<CerealData> GetAllCereals();

    CerealData GetCereal(int id);

    byte[] GetImage(int id);

    void CreateCereal(CerealData cereal);

    void UpdateCereal(CerealData cereal);

    void DeleteCereal(int id);
}
