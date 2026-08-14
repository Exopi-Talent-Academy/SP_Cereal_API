using Cereal_API.Shared.Models;

namespace Cereal_API.API.Services;

public interface ICerealService
{
    List<CerealData> GetAllCereals();

    CerealData GetCereal(int id);

    void GetImage(int id);  // Unsure about the return type here

    void CreateCereal(CerealData cereal);

    void UpdateCereal(CerealData cereal);

    void DeleteCereal(int id);
}
