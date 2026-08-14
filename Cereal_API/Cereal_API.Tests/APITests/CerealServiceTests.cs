using Cereal_API.API.Services;
using Cereal_API.Shared.Models;

namespace Cereal_API.Tests.APITests;

[TestClass]
public sealed class CerealServiceTests
{
    [TestMethod]
    public void GetAllCereals_ReturnsAllCereals()
    {
        // Arrange
        using var context = TestHelper.CreateContext();
        context.AddRange(
            new CerealData { Id = 1, Name = "A" },
            new CerealData { Id = 2, Name = "B" },
            new CerealData { Id = 3, Name = "C" }
            );
        context.SaveChanges();

        var service = new CerealService(context);

        // Act
        var actual = service.GetAllCereals();

        // Assert
        Assert.HasCount(3, actual);
    }

    [TestMethod]
    public void GetCereal_ExistingId_ReturnsCereal()
    {
        // Arrange
        int expectedId = 15;
        string expectedName = "A";

        using var context = TestHelper.CreateContext();
        context.Add(new CerealData { Id = expectedId, Name = expectedName });
        context.SaveChanges();

        var service = new CerealService(context);

        // Act
        CerealData actual = service.GetCereal(15);

        // Assert
        Assert.IsNotNull(actual);
        Assert.AreEqual(expectedId, actual.Id);
        Assert.AreEqual(expectedName, actual.Name);
    }

    [TestMethod]
    public void GetCereal_InvalidId_ThrowsKeyNotFound()
    {
        // Arrange
        using var context = TestHelper.CreateContext();
        var service = new CerealService(context);

        // Act & Assert
        Assert.ThrowsExactly<KeyNotFoundException>(() => service.GetCereal(1));
    }

    [TestMethod]
    public void CreateCereal_ValidData_AddsCereal()
    {
        // Arrange
        int expectedId = 1;
        string expectedName = "A";
        CerealData newCereal = new() { Id = expectedId, Name = expectedName };

        using var context = TestHelper.CreateContext();
        context.SaveChanges();

        var service = new CerealService(context);

        // Act
        service.CreateCereal(newCereal);
        var all = service.GetAllCereals();

        // Assert
        Assert.IsNotNull(all);
        Assert.HasCount(1, all);
        Assert.AreEqual(expectedName, all[0].Name);
        Assert.AreEqual(expectedId, all[0].Id);
        Assert.AreNotEqual(0, all[0].Id);
    }

    /*[TestMethod]
    public void CreateCereal_InvalidData_ThrowsArgumentException()
    {
        var svc = new FakeCerealService();
        var cereal = Sample(0, "");
        AssertThrows<ArgumentException>(() => svc.CreateCereal(cereal));
    }*/

    /*[TestMethod]
    public void UpdateCereal_ExistingId_UpdatesValues()
    {
        var svc = new FakeCerealService(new[] { Sample(1, "Old") });
        var updated = Sample(1, "Updated");
        svc.UpdateCereal(updated);
        var c = svc.GetCereal(1);
        Assert.AreEqual("Updated", c.Name);
    }*/

    /*[TestMethod]
    public void DeleteCereal_ExistingId_RemovesCereal()
    {
        var svc = new FakeCerealService(new[] { Sample(1, "ToDelete") });
        svc.DeleteCereal(1);
        AssertThrows<KeyNotFoundException>(() => svc.GetCereal(1));
    }*/

    /*[TestMethod]
    public void DeleteCereal_InvalidId_ThrowsKeyNotFound()
    {
        var svc = new FakeCerealService();
        AssertThrows<KeyNotFoundException>(() => svc.DeleteCereal(123));
    }*/

    private static void AssertThrows<T>(Action action) where T : Exception
    {
        try
        {
            action();
            Assert.Fail($"Expected exception of type {typeof(T).Name} but no exception was thrown.");
        }
        catch (T)
        {
            // expected
        }
    }
}
