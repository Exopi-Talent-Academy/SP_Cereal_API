using Cereal_API.Shared.Models;

namespace Cereal_API.API.Services;

public interface ICerealService
{
    List<CerealData> GetAllCereals();

    CerealData GetCereal(int id);

    (byte[] Data, string ContentType) GetImage(int id);

    void CreateCereal(CerealData cereal);

    void CreateCereals(List<CerealData> cereals);

    void UpdateCereal(CerealData cereal);

    void DeleteCereal(int id);

    void RestoreCereal(int id);
}
