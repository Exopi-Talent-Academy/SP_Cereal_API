using Cereal_API.Shared.Models;
using Cereal_API.Tests.Helper;
using Cereal_API.WPF.Services;
using System.Net;
using System.Text;

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
    public async Task Login_Authorized_ReturnsNotNull()
    {
        // Arrange
        AuthenticationResult expectedResult = new()
        {
            Username = "admin",
            Role = "Admin"
        };
        var handler = FakeHttpMessageHandler.CreateFor(expectedResult);
        var client = CreateClient(handler);
        var service = new CerealHttpService(client);

        // Act
        var actual = await service.Login(expectedResult.Username, "password");

        // Assert
        Assert.IsNotNull(actual);
        Assert.AreEqual(expectedResult.Username, actual.Username);
        Assert.AreEqual(expectedResult.Role, actual.Role);
        Assert.AreEqual(HttpMethod.Get, handler.Request!.Method);
        Assert.AreEqual(baseUrl + CerealHttpService.URL_LOGIN_CHECK, handler.Request.RequestUri!.ToString());
    }

    [TestMethod]
    public async Task Login_Unauthorized_ReturnsNull()
    {
        // Arrange
        var handler = FakeHttpMessageHandler.Create(HttpStatusCode.Unauthorized);
        var client = CreateClient(handler);
        var service = new CerealHttpService(client);

        // Act
        var actual = await service.Login("admin", "password");

        // Assert
        Assert.IsNull(actual);
    }

    [TestMethod]
    public async Task Login_Authorized_ValidAuthorization()
    {
        // Arrange
        AuthenticationResult expectedResult = new()
        {
            Username = "admin",
            Role = "Admin"
        };
        var password = "password";
        var handler = FakeHttpMessageHandler.CreateFor(expectedResult);
        var client = CreateClient(handler);
        var service = new CerealHttpService(client);

        // Act
        await service.Login(expectedResult.Username, password);

        // Assert
        Assert.IsNotNull(client.DefaultRequestHeaders.Authorization);
        Assert.AreEqual("Basic", client.DefaultRequestHeaders.Authorization!.Scheme);
        var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(client.DefaultRequestHeaders.Authorization.Parameter!));
        Assert.AreEqual($"{expectedResult.Username}:{password}", decoded);
    }

    [TestMethod]
    public async Task Logout_NullAuthorization()
    {
        // Arrange
        var handler = FakeHttpMessageHandler.Create();
        var client = CreateClient(handler);
        var service = new CerealHttpService(client);

        // Act
        await service.Logout();

        // Assert
        Assert.IsNull(client.DefaultRequestHeaders.Authorization);
    }

    [TestMethod]
    public async Task GetAllCereals_GETRequest_ReturnsCereals()
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
        
        // Assert
        Assert.HasCount(cereals.Count, actual);
        Assert.AreEqual(HttpMethod.Get, handler.Request!.Method);
        Assert.AreEqual(baseUrl + CerealHttpService.URL, handler.Request.RequestUri!.ToString());
    }

    [TestMethod]
    public async Task GetAllCereals_Exception_ReturnsEmptyList()
    {
        // Arrange
        var cereals = new List<CerealData>
        {
            new() { Id = 1, Name = "A" },
            new() { Id = 2, Name = "B" },
            new() { Id = 3, Name = "C" }
        };

        var handler = FakeHttpMessageHandler.CreateFor(cereals, HttpStatusCode.NotFound);
        var client = CreateClient(handler);
        var service = new CerealHttpService(client);

        // Act
        var actual = await service.GetAllCereals();

        // Assert
        Assert.IsEmpty(actual);
    }

    [TestMethod]
    public async Task GetCereal_GETRequest_ReturnsCereal()
    {
        // Arrange
        var expectedId = 1;
        var cereal = new CerealData { Id = expectedId, Name = "A" };

        var handler = FakeHttpMessageHandler.CreateFor(cereal);
        var client = CreateClient(handler);
        var service = new CerealHttpService(client);

        // Act
        var actual = await service.GetCereal(expectedId);

        // Assert
        Assert.AreEqual(cereal.Id, actual.Id);
        Assert.AreEqual(cereal.Name, actual.Name);
        Assert.AreEqual(HttpMethod.Get, handler.Request!.Method);
        Assert.AreEqual(baseUrl + CerealHttpService.URL_ID(expectedId), handler.Request.RequestUri!.ToString());
    }

    [TestMethod]
    public async Task GetCereal_Exception_ReturnsNewCereal()
    {
        // Arrange
        var id = 1;
        var cereal = new CerealData { Id = id, Name = "A" };
        var emptyCereal = new CerealData();

        var handler = FakeHttpMessageHandler.CreateFor(cereal, HttpStatusCode.NotFound);
        var client = CreateClient(handler);
        var service = new CerealHttpService(client);

        // Act
        var actual = await service.GetCereal(id);

        // Assert
        Assert.AreEqual(emptyCereal.Id, actual.Id);
        Assert.AreEqual(emptyCereal.Name, actual.Name);
    }

    [TestMethod]
    public async Task GetCerealImage_GETRequest_ReturnsBytes()
    {
        // Arrange
        var id = 1;
        byte[] image = [1, 2, 3, 4, 5];

        var handler = FakeHttpMessageHandler.CreateFor(image);
        var client = CreateClient(handler);
        var service = new CerealHttpService(client);

        // Act
        var actual = await service.GetCerealImage(id);

        // Assert
        Assert.HasCount(image.Length, actual);
        Assert.AreEqual(HttpMethod.Get, handler.Request!.Method);
        Assert.AreEqual(baseUrl + CerealHttpService.URL_ID_IMAGE(id), handler.Request.RequestUri!.ToString());
    }

    [TestMethod]
    public async Task GetCerealImage_Exception_ReturnsEmptyBytes()
    {
        // Arrange
        var id = 1;
        byte[] image = [1, 2, 3, 4, 5];

        var handler = FakeHttpMessageHandler.CreateFor(image, HttpStatusCode.NotFound);
        var client = CreateClient(handler);
        var service = new CerealHttpService(client);

        // Act
        var actual = await service.GetCerealImage(id);

        // Assert
        Assert.IsEmpty(actual);
    }

    [TestMethod]
    public async Task CreateCereal_POSTRequest_ReturnsCreatedCereal()
    {
        // Arrange
        var cereal = new CerealData { Id = 0, Name = "A Very Long And Convoluted Name" };

        var handler = FakeHttpMessageHandler.CreateFor(cereal);
        var client = CreateClient(handler);
        var service = new CerealHttpService(client);

        // Act
        var actual = await service.CreateCereal(cereal);

        // Assert
        Assert.AreEqual(HttpMethod.Post, handler.Request!.Method);
        Assert.AreEqual(baseUrl + CerealHttpService.URL, handler.Request.RequestUri!.ToString());
        
        Assert.AreEqual(cereal.Name, actual.Name);
    }

    [TestMethod]
    public async Task CreateCereal_Exception_ReturnsException()
    {
        // Arrange
        var cereal = new CerealData { Id = 0, Name = "A" };

        var handler = FakeHttpMessageHandler.Create(HttpStatusCode.BadRequest);
        var client = CreateClient(handler);
        var service = new CerealHttpService(client);

        // Act & Assert
        await Assert.ThrowsExactlyAsync<HttpRequestException>(() => service.CreateCereal(cereal));
    }

    [TestMethod]
    [DataRow("CSV Valid.csv", 0)]
    [DataRow("CSV Validation Errors.csv", 4)]
    public async Task CreateCereals_POSTRequest_ReturnsErrorReports(string csvFileName, int expectedErrorReports)
    {
        // Arrange
        List<ErrorReport> reports = [];
        for (int i = 0; i < expectedErrorReports; i++)
        {
            reports.Add(new ErrorReport("", ""));
        }
        var handler = FakeHttpMessageHandler.CreateFor(reports);
        var client = CreateClient(handler);
        var service = new CerealHttpService(client);

        // Act
        var errorReports = await service.CreateCereals(MockFilesHelper.GetMockFilePath(csvFileName));

        // Assert
        Assert.AreEqual(HttpMethod.Post, handler.Request!.Method);
        Assert.AreEqual(baseUrl + CerealHttpService.URL_BULK, handler.Request.RequestUri!.ToString());

        Assert.IsNotNull(errorReports);
        Assert.HasCount(expectedErrorReports, errorReports);
    }

    [TestMethod]
    public async Task UpdateCereal_PUTRequest_ReturnsUpdatedCereal()
    {
        // Arrange
        var cereal = new CerealData { Id = 1, Name = "A Very Long And Convoluted Name" };

        var handler = FakeHttpMessageHandler.CreateFor(cereal);
        var client = CreateClient(handler);
        var service = new CerealHttpService(client);

        // Act
        var actual = await service.UpdateCereal(cereal);

        // Assert
        Assert.AreEqual(HttpMethod.Put, handler.Request!.Method);
        Assert.AreEqual(baseUrl + CerealHttpService.URL, handler.Request.RequestUri!.ToString());

        Assert.AreEqual(cereal.Name, actual.Name);
    }

    [TestMethod]
    public async Task UpdateCereal_Exception_ReturnsException()
    {
        // Arrange
        var cereal = new CerealData { Id = 1, Name = "A" };

        var handler = FakeHttpMessageHandler.Create(HttpStatusCode.NotFound);
        var client = CreateClient(handler);
        var service = new CerealHttpService(client);

        // Act & Assert
        await Assert.ThrowsExactlyAsync<HttpRequestException>(() => service.UpdateCereal(cereal));
    }

    [TestMethod]
    public async Task DeleteCereal_DELETERequest_ReturnsMarkedAsDeletedCereal()
    {
        // Arrange
        var id = 1;
        var cereal = new CerealData { Id = id, Name = "A", MarkedAsDeleted = true };

        var handler = FakeHttpMessageHandler.CreateFor(cereal);
        var client = CreateClient(handler);
        var service = new CerealHttpService(client);

        // Act
        var actual = await service.DeleteCereal(id);

        // Assert
        Assert.AreEqual(HttpMethod.Delete, handler.Request!.Method);
        Assert.AreEqual(baseUrl + CerealHttpService.URL_ID(id), handler.Request.RequestUri!.ToString());

        Assert.AreEqual(cereal.MarkedAsDeleted, actual.MarkedAsDeleted);
    }

    [TestMethod]
    public async Task DeleteCereal_Exception_ReturnsException()
    {
        // Arrange
        var handler = FakeHttpMessageHandler.Create(HttpStatusCode.NotFound);
        var client = CreateClient(handler);
        var service = new CerealHttpService(client);

        // Act & Assert
        await Assert.ThrowsExactlyAsync<HttpRequestException>(() => service.DeleteCereal(1));
    }

    [TestMethod]
    public async Task RestoreCereal_POSTRequest_ReturnsSucceeds()
    {
        // Arrange
        var id = 1;
        var cereal = new CerealData { Id = id, Name = "A", MarkedAsDeleted = false };

        var handler = FakeHttpMessageHandler.CreateFor(cereal);
        var client = CreateClient(handler);
        var service = new CerealHttpService(client);

        // Act
        var actual = await service.RestoreCereal(id);

        // Assert
        Assert.AreEqual(HttpMethod.Post, handler.Request!.Method);
        Assert.AreEqual(baseUrl + CerealHttpService.URL_ID_RESTORE(id), handler.Request.RequestUri!.ToString());

        Assert.AreEqual(cereal.MarkedAsDeleted, actual.MarkedAsDeleted);
    }

    [TestMethod]
    public async Task RestoreCereal_Exception_ReturnsException()
    {
        // Arrange
        var handler = FakeHttpMessageHandler.Create(HttpStatusCode.NotFound);
        var client = CreateClient(handler);
        var service = new CerealHttpService(client);

        // Act & Assert
        await Assert.ThrowsExactlyAsync<HttpRequestException>(() => service.RestoreCereal(1));
    }
}
