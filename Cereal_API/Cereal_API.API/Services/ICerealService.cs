using Cereal_API.Shared.Models;

namespace Cereal_API.API.Services;

public interface ICerealService
{
    List<CerealData> GetAllCereals();

    CerealData GetCereal(int id);

    (byte[] Data, string ContentType) GetImage(int id);

    CerealData CreateCereal(CerealData cereal);

    void CreateCereals(List<CerealData> cereals);

    CerealData UpdateCereal(CerealData cereal);

    CerealData DeleteCereal(int id);

    CerealData RestoreCereal(int id);
}
