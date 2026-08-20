using Cereal_API.Shared.Models;
using Cereal_API.Tests.Helper;
using Cereal_API.WPF.Services;
using System.Net;

namespace Cereal_API.Tests.WPFTests;

[TestClass]
public class CerealHttpServiceTests
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
    public async Task GetAllCereals_ReturnsCerealsAndGETRequest()   // 404/500 handling?
    {
        // Arrange
        var cereals = new List<CerealData>
        {
            new() { Id = 1, Name = "A" },
            new() { Id = 2, Name = "B" },
            new() { Id = 3, Name = "C" }
        };

        var handler = FakeHttpMessageHandler.CreateFor(cereals);
        var client = CreateClient(handler);
        var service = new CerealHttpService(client);

        // Act
        var actual = await service.GetAllCereals();
        
        //Assert
        Assert.HasCount(3, actual);
        Assert.AreEqual(HttpMethod.Get, handler.Request!.Method);
        Assert.AreEqual(baseUrl + CerealHttpService.URL, handler.Request.RequestUri!.ToString());
    }

    [TestMethod]
    public async Task GetCereal_ReturnsCerealAndGETRequest()   // 404 handling?
    {
        // Arrange
        var expectedId = 1;
        var cereal = new CerealData { Id = expectedId, Name = "A" };

        var handler = FakeHttpMessageHandler.CreateFor(cereal);
        var client = CreateClient(handler);
        var service = new CerealHttpService(client);

        // Act
        var actual = await service.GetCereal(expectedId);

        //Assert
        Assert.AreEqual(cereal.Id, actual.Id);
        Assert.AreEqual(cereal.Name, actual.Name);
        Assert.AreEqual(HttpMethod.Get, handler.Request!.Method);
        Assert.AreEqual(baseUrl + CerealHttpService.URL_ID(expectedId), handler.Request.RequestUri!.ToString());
    }

    [TestMethod]
    public async Task GetCerealImage_ReturnsBytesAndGETRequest()    // 404 handling?
    {
        // Arrange
        var id = 1;
        byte[] image = [1, 2, 3, 4, 5];

        var handler = FakeHttpMessageHandler.CreateFor(image);
        var client = CreateClient(handler);
        var service = new CerealHttpService(client);

        // Act
        var actual = await service.GetCerealImage(id);

        //Assert
        Assert.HasCount(image.Length, actual);
        Assert.AreEqual(HttpMethod.Get, handler.Request!.Method);
        Assert.AreEqual(baseUrl + CerealHttpService.URL_ID_IMAGE(id), handler.Request.RequestUri!.ToString());
    }

    [TestMethod]
    public async Task CreateCereal_ReturnsPOSTRequestAndJSONBody() // 400 handling?
    {
        // Arrange
        var expectedName = "A Very Long And Convoluted Name";
        var cereal = new CerealData { Id = 0, Name = expectedName };

        var handler = FakeHttpMessageHandler.Create();
        var client = CreateClient(handler);
        var service = new CerealHttpService(client);

        // Act
        await service.CreateCereal(cereal);

        //Assert
        Assert.AreEqual(HttpMethod.Post, handler.Request!.Method);
        Assert.AreEqual(baseUrl + CerealHttpService.URL, handler.Request.RequestUri!.ToString());
        
        var json = await handler.Request.Content!.ReadAsStringAsync(CancellationToken.None);
        Assert.Contains(expectedName, json);
    }

    [TestMethod]
    public async Task UpdateCereal_ReturnsPUTRequestAndJSONBody()  // 404 handling?
    {
        // Arrange
        var expectedName = "A Very Long And Convoluted Name";
        var cereal = new CerealData { Id = 1, Name = expectedName };

        var handler = FakeHttpMessageHandler.Create();
        var client = CreateClient(handler);
        var service = new CerealHttpService(client);

        // Act
        await service.UpdateCereal(cereal);

        //Assert
        Assert.AreEqual(HttpMethod.Put, handler.Request!.Method);
        Assert.AreEqual(baseUrl + CerealHttpService.URL, handler.Request.RequestUri!.ToString());

        var json = await handler.Request.Content!.ReadAsStringAsync(CancellationToken.None);
        Assert.Contains(expectedName, json);
    }

    [TestMethod]
    public async Task DeleteCereal_ReturnsDELETERequestAndSucceeds()    // 404 handling?
    {
        // Arrange
        var id = 1;

        var handler = FakeHttpMessageHandler.Create(HttpStatusCode.NoContent);
        var client = CreateClient(handler);
        var service = new CerealHttpService(client);

        // Act
        await service.DeleteCereal(id);

        //Assert
        Assert.AreEqual(HttpMethod.Delete, handler.Request!.Method);
        Assert.AreEqual(baseUrl + CerealHttpService.URL_ID(id), handler.Request.RequestUri!.ToString());
    }
}
