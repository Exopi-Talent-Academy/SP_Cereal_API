using Cereal_API.Shared;
using Cereal_API.Shared.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace Cereal_API.WPF.Services;

public class CerealHttpService(HttpClient httpClient)
{
    private readonly HttpClient client = httpClient;

    public const string URL = "cerealapi";
    public static string URL_ID(int id) => $"{URL}/{id}";
    public static string URL_ID_IMAGE(int id) => $"{URL_ID(id)}/image";

    //var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

    public async Task<List<CerealData>> GetAllCereals()
    {
        try
        {
            using var response = await client.GetAsync(URL);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<CerealData>>() ?? [];
        }
        catch (Exception ex)
        {
            DebugHelper.ReportException(nameof(GetAllCereals), ex);
            return [];
        }
    }

    public async Task<CerealData> GetCereal(int id)
    {
        try
        {
            using var response = await client.GetAsync(URL_ID(id));
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CerealData>() ?? new();
        }
        catch (Exception ex)
        {
            DebugHelper.ReportException(nameof(GetCereal), ex);
            return new();
        }
    }

    public async Task<byte[]> GetCerealImage(int id)
    {
        try
        {
            using var response = await client.GetAsync(URL_ID_IMAGE(id));
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();
        }
        catch (Exception ex)
        {
            DebugHelper.ReportException(nameof(GetCerealImage), ex);
            return [];
        }
    }

    public async Task CreateCereal(CerealData cereal)
    {
        using var response = await client.PostAsJsonAsync(URL, cereal);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateCereal(CerealData cereal)
    {
        using var response = await client.PutAsJsonAsync(URL, cereal);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteCereal(int id)
    {
        using var response = await client.DeleteAsync(URL_ID(id));
        response.EnsureSuccessStatusCode();
    }
}
