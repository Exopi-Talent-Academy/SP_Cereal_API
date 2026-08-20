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
        throw new NotImplementedException();
        //var response = await client.GetAsync(URL);
        //response.EnsureSuccessStatusCode();

        //return await response.Content.ReadFromJsonAsync<List<CerealData>>() ?? [];
    }

    // GET
    // var users = await client.GetFromJsonAsync<List<User>>(url);
    public async Task<CerealData> GetCereal(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<byte[]> GetCerealImage(int id)    // Unsure if this is the right return type
    {
        throw new NotImplementedException();
    }

    // POST
    // await client.PostAsJsonAsync(url, user);
    public async Task CreateCereal(CerealData cereal)
    {
        throw new NotImplementedException();
    }

    // PUT
    // await client.PutAsJsonAsync(url, user);
    public async Task UpdateCereal(CerealData cereal)
    {
        throw new NotImplementedException();
    }

    // DELETE
    // await client.DeleteAsync(url);
    public async Task DeleteCereal(int id)
    {
        throw new NotImplementedException();
    }
}
