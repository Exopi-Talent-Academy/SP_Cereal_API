using Cereal_API.Shared.Models;
using Cereal_API.WPF.Services;

namespace Cereal_API.Tests.WPFTests;

[TestClass]
public class CerealApiServiceTests
{
    private const string baseUrl = "http://localhost/";

    private static HttpClient CreateClient(HttpMessageHandler handler)
    {
        return new HttpClient(handler)
        {
            BaseAddress = new Uri(baseUrl)
        };
    }

    [TestMethod]
    public async Task GetAllCereals_ReturnsGETRequestAndURL()   // 404/500 handling?
    {
        // Arrange
        var cereals = new List<CerealData>
        {
            new() { Id = 1, Name = "A" },
            new() { Id = 2, Name = "B" },
            new() { Id = 3, Name = "C" }
        };

        var handler = FakeHttpMessageHandler.CreateFakeMessageHandlerFor(cereals);
        var client = CreateClient(handler);
        var service = new CerealApiService(client);

        // Act
        var actual = await service.GetAllCereals();
        
        //Assert
        Assert.HasCount(3, actual);
        Assert.AreEqual(HttpMethod.Get, handler.Request!.Method);
        Assert.AreEqual(baseUrl + CerealApiService.URL, handler.Request.RequestUri!.ToString());
    }

    [TestMethod]
    public async Task GetCereal_ReturnsGETRequestAndURL()   // 404 handling?
    {
        // Arrange
        var expectedId = 1;
        var cereal = new CerealData { Id = expectedId, Name = "A" };

        var handler = FakeHttpMessageHandler.CreateFakeMessageHandlerFor(cereal);
        var client = CreateClient(handler);
        var service = new CerealApiService(client);

        // Act
        var actual = await service.GetCereal(expectedId);

        //Assert
        Assert.AreEqual(cereal.Id, actual.Id);
        Assert.AreEqual(cereal.Name, actual.Name);
        Assert.AreEqual(HttpMethod.Get, handler.Request!.Method);
        Assert.AreEqual(baseUrl + CerealApiService.URL_ID(expectedId), handler.Request.RequestUri!.ToString());
    }

    [TestMethod]
    public async Task GetCerealImage_ReturnsGETRequestAndBytes()    // 404 handling?
    {
        throw new NotImplementedException();
    }

    [TestMethod]
    public async Task CreateCereal_ReturnsPOSTRequestAndSucceeds(CerealData cereal) // 400 handling?
    {
        throw new NotImplementedException();
    }

    [TestMethod]
    public async Task UpdateCereal_ReturnsPUTRequestAndSucceeds(CerealData cereal)  // 404 handling?
    {
        throw new NotImplementedException();
    }

    [TestMethod]
    public async Task DeleteCereal_ReturnsDELETERequestAndSucceeds()    // 404 handling?
    {
        throw new NotImplementedException();
    }
}
