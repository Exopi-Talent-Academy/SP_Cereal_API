using Cereal_API.Shared;
using Cereal_API.Shared.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace Cereal_API.WPF.Services;

public class CerealHttpService(HttpClient httpClient)
{
    private readonly HttpClient client = httpClient;

    public const string URL = "api/cereal";
    public const string URL_LOGIN_CHECK = $"{URL}/auth/check";
    public static string URL_ID(int id) => $"{URL}/{id}";
    public static string URL_ID_IMAGE(int id) => $"{URL_ID(id)}/image";

    public async Task<AuthenticationResult?> Login(string username, string password)
    {
        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

        using var response = await client.GetAsync(URL_LOGIN_CHECK);

        if (!response.IsSuccessStatusCode)
        {
            client.DefaultRequestHeaders.Authorization = null;
            return null;
        }

        return await response.Content.ReadFromJsonAsync<AuthenticationResult>();
    }

    public async Task Logout()
    {
        client.DefaultRequestHeaders.Authorization = null;
    }

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
            DebugHelper.ReportException(ex);
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
            DebugHelper.ReportException(ex);
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
            DebugHelper.ReportException(ex);
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
