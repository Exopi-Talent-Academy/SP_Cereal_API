using Cereal_API.API.Services;
using Cereal_API.Shared;
using Cereal_API.Shared.Models;
using Cereal_API.Tests.Helper;

namespace Cereal_API.Tests.APITests;

[TestClass]
public sealed class CerealServiceTests
{
    [TestMethod]
    public void GetAllCereals_ReturnsAllCereals()
    {
        // Arrange
        using var context = FakeDbContext.Create();
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
        Assert.IsNotNull(actual);
        Assert.HasCount(3, actual);
    }

    [TestMethod]
    public void GetCereal_ExistingId_ReturnsCereal()
    {
        // Arrange
        int expectedId = 15;
        string expectedName = "A";

        using var context = FakeDbContext.Create();
        context.Add(new CerealData { Id = expectedId, Name = expectedName });
        context.SaveChanges();

        var service = new CerealService(context);

        // Act
        CerealData actual = service.GetCereal(expectedId);

        // Assert
        Assert.IsNotNull(actual);
        Assert.AreEqual(expectedId, actual.Id);
        Assert.AreEqual(expectedName, actual.Name);
    }

    [TestMethod]
    public void GetCereal_InvalidId_ThrowsKeyNotFound()
    {
        // Arrange
        using var context = FakeDbContext.Create();
        var service = new CerealService(context);

        // Act & Assert
        Assert.ThrowsExactly<KeyNotFoundException>(() => service.GetCereal(1));
    }

    [TestMethod]
    public void GetImage_ValidId_ReturnsImage()
    {
        // Arrange
        int id = 1;
        string name = "Test";

        using var context = FakeDbContext.Create();
        context.Add(new CerealData { Id = id, Name = name });
        context.SaveChanges();

        var service = new CerealService(context, FakeDbContext.ImageFolderPath);

        // Act
        var image = service.GetImage(id);

        // Assert
        Assert.IsNotNull(image);
        Assert.IsNotEmpty(image);
    }

    [TestMethod]
    public void GetImage_ValidIdButMissingImage_ThrowsFileNotFound()
    {
        // Arrange
        int id = 1;
        string name = "A";

        using var context = FakeDbContext.Create();
        context.Add(new CerealData { Id = id, Name = name });
        context.SaveChanges();

        var service = new CerealService(context, FakeDbContext.ImageFolderPath);

        // Act & Assert
        Assert.ThrowsExactly<FileNotFoundException>(() => service.GetImage(id));
    }

    [TestMethod]
    public void GetImage_InvalidId_ThrowsKeyNotFound()
    {
        // Arrange
        using var context = FakeDbContext.Create();
        context.SaveChanges();

        var service = new CerealService(context, FakeDbContext.ImageFolderPath);

        // Act & Assert
        Assert.ThrowsExactly<KeyNotFoundException>(() => service.GetImage(1));
    }

    [TestMethod]
    [DataRow(0)]    // Valid Id
    [DataRow(15)]   // Invalid Id
    public void CreateCereal_AddsCereal(int id)
    {
        // Arrange
        int expectedId = 1;
        string expectedName = "A";
        CerealData newCereal = new() { Id = id, Name = expectedName };

        using var context = FakeDbContext.Create();
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
        Assert.AreNotEqual(id, all[0].Id);
    }

    [TestMethod]
    [DataRow((CerealEnums.Manufacturer)999, (CerealEnums.Type)0)]
    [DataRow((CerealEnums.Manufacturer)0, (CerealEnums.Type)999)]
    [DataRow((CerealEnums.Manufacturer)999, (CerealEnums.Type)999)]
    public void CreateCereal_InvalidEnums_ThrowsArgumentException(CerealEnums.Manufacturer manufacturer, CerealEnums.Type type)
    {
        // Arrange
        CerealData newCereal = new() { Id = 0, Name = "A", Manufacturer = manufacturer, Type = type };

        using var context = FakeDbContext.Create();
        context.SaveChanges();

        var service = new CerealService(context);

        // Act & Assert
        Assert.ThrowsExactly<ArgumentException>(() => service.CreateCereal(newCereal));
    }

    [TestMethod]
    public void UpdateCereal_ExistingId_UpdatesValues()
    {
        // Arrange
        int expectedId = 1;
        string oldName = "Old";
        string updatedName = "Updated";
        CerealData newCereal = new() { Id = expectedId, Name = updatedName };

        using var context = FakeDbContext.Create();
        context.Add(new CerealData { Id = expectedId, Name = oldName });
        context.SaveChanges();

        var service = new CerealService(context);

        // Act
        service.UpdateCereal(newCereal);
        var result = service.GetCereal(expectedId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(updatedName, result.Name);
        Assert.AreEqual(expectedId, result.Id);
        Assert.AreNotEqual(oldName, result.Name);
    }

    [TestMethod]
    public void UpdateCereal_InvalidId_ThrowsKeyNotFound()
    {
        // Arrange
        CerealData newCereal = new() { Id = 1, Name = "Updated" };

        using var context = FakeDbContext.Create();
        context.SaveChanges();

        var service = new CerealService(context);

        // Act & Assert
        Assert.ThrowsExactly<KeyNotFoundException>(() => service.UpdateCereal(newCereal));
    }

    [TestMethod]
    public void DeleteCereal_ExistingId_RemovesCereal()
    {
        // Arrange
        int deletedId = 2;

        using var context = FakeDbContext.Create();
        context.AddRange(
            new CerealData { Id = 1, Name = "A" },
            new CerealData { Id = 2, Name = "B" },
            new CerealData { Id = 3, Name = "C" }
            );
        context.SaveChanges();

        var service = new CerealService(context);

        // Act
        service.DeleteCereal(deletedId);
        var all = service.GetAllCereals();

        // Assert
        Assert.IsNotNull(all);
        Assert.HasCount(2, all);
        Assert.AreEqual(1, all[0].Id);
        Assert.AreEqual(3, all[1].Id);
        Assert.AreNotEqual(deletedId, all[1].Id);
        Assert.ThrowsExactly<KeyNotFoundException>(() => service.GetCereal(deletedId));
    }

    [TestMethod]
    public void DeleteCereal_InvalidId_ThrowsKeyNotFound()
    {
        // Arrange
        using var context = FakeDbContext.Create();
        var service = new CerealService(context);

        // Act & Assert
        Assert.ThrowsExactly<KeyNotFoundException>(() => service.DeleteCereal(1));
    }
}
