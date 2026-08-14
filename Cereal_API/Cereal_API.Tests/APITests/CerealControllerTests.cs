using Cereal_API.API.Controllers;
using Cereal_API.API.Services;
using Cereal_API.Shared;
using Cereal_API.Shared.Models;
using Moq;

namespace Cereal_API.Tests.APITests;

[TestClass]
public class CerealControllerTests
{
    // Consider if these tests are even needed (other to check that they are calling the Service...
    // Otherwise should the WPF HttpClient tests cover the API calls

    /*[TestMethod]
    public void GetAllCereals_ReturnsAllCereals()
    {
        // Arrange
        using var context = TestHelper.CreateContext();
        context.AddRange(
            new CerealData
            {
                Id = 1,
                Name = "A"
            },
            new CerealData
            {
                Id = 2,
                Name = "B"
            },
            new CerealData
            {
                Id = 3,
                Name = "C"
            });
        context.SaveChanges();

        var service = new CerealService(context);
        var controller = new CerealController(service);

        // Act
        var actual = controller.GetAll();

        // Assert
        Assert.AreEqual(3, actual.Count());
    }

    [TestMethod]
    public void GetCereal_ExistingId_ReturnsCereal()
    {
        // Arrange
        int expectedId = 15;
        string expectedName = "A";
        using var context = TestHelper.CreateContext();
        context.AddRange(
            new CerealData
            {
                Id = expectedId,
                Name = expectedName
            });
        context.SaveChanges();

        var repository = new CerealRepository(context);
        var controller = new CerealController(repository);

        // Act
        CerealData actual = controller.Get(15);

        // Assert
        Assert.IsNotNull(actual);
        Assert.AreEqual(expectedId, actual.Id);
        Assert.AreEqual(expectedName, actual.Name);
    }*/
}
