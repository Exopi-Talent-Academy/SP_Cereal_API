using Cereal_API.API.Services;
using Cereal_API.Shared;
using Cereal_API.Shared.DTOs;
using Cereal_API.Shared.Models;
using Cereal_API.Tests.Helper;
using Microsoft.AspNetCore.Http;
using Moq;

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

        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        var service = new CerealService(context, httpContextAccessor.Object);

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

        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        var service = new CerealService(context, httpContextAccessor.Object);

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
        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        var service = new CerealService(context, httpContextAccessor.Object);

        // Act & Assert
        Assert.ThrowsExactly<KeyNotFoundException>(() => service.GetCereal(1));
    }

    [TestMethod]
    [DataRow("JPG_Test", "image/jpeg")]
    [DataRow("PNG_Test", "image/png")]
    public void GetImage_ValidId_ReturnsImage(string fileName, string expectedContentType)
    {
        // Arrange
        int id = 1;

        using var context = FakeDbContext.Create();
        context.Add(new CerealData { Id = id, Name = fileName });
        context.SaveChanges();

        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        var service = new CerealService(context, httpContextAccessor.Object, MockFilesHelper.MockFolderPath);

        // Act
        var (Data, ContentType) = service.GetImage(id);

        // Assert
        Assert.IsNotNull(Data);
        Assert.IsNotEmpty(Data);
        Assert.AreEqual(expectedContentType, ContentType);
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

        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        var service = new CerealService(context, httpContextAccessor.Object, MockFilesHelper.MockFolderPath);

        // Act & Assert
        Assert.ThrowsExactly<FileNotFoundException>(() => service.GetImage(id));
    }

    [TestMethod]
    public void GetImage_InvalidId_ThrowsKeyNotFound()
    {
        // Arrange
        using var context = FakeDbContext.Create();
        context.SaveChanges();

        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        var service = new CerealService(context, httpContextAccessor.Object);

        // Act & Assert
        Assert.ThrowsExactly<KeyNotFoundException>(() => service.GetImage(1));
    }

    [TestMethod]
    public void CreateCereal_AddsCereal()
    {
        // Arrange
        int expectedId = 1;
        string expectedName = "A";
        CreateCerealDTO createdCereal = new() { Name = expectedName };

        using var context = FakeDbContext.Create();
        context.SaveChanges();

        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        var service = new CerealService(context, httpContextAccessor.Object);

        // Act
        var actual = service.CreateCereal(createdCereal);

        // Assert
        Assert.IsNotNull(actual);
        Assert.AreEqual(expectedName, actual.Name);
        Assert.AreEqual(expectedId, actual.Id);
        Assert.AreEqual(DateTime.UtcNow.Date, actual.DbVersion?.Date);
    }

    [TestMethod]
    [DataRow("", (CerealEnums.Manufacturer)0, (CerealEnums.Type)0, 100)]    // Invalid Name
    [DataRow("A", (CerealEnums.Manufacturer)999, (CerealEnums.Type)0, 100)] // Invalid Manufacturer
    [DataRow("A", (CerealEnums.Manufacturer)0, (CerealEnums.Type)999, 100)] // Invalid Type
    [DataRow("A", (CerealEnums.Manufacturer)0, (CerealEnums.Type)0, -1)]    // Invalid Number
    public void CreateCereal_FailingValidation_ThrowsArgumentException(string name, CerealEnums.Manufacturer manufacturer, CerealEnums.Type type, int calories)
    {
        // Arrange
        CreateCerealDTO createdCereal = new() { Name = name, Manufacturer = manufacturer, Type = type, Calories = calories };

        using var context = FakeDbContext.Create();
        context.SaveChanges();

        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        var service = new CerealService(context, httpContextAccessor.Object);

        // Act & Assert
        Assert.ThrowsExactly<ArgumentException>(() => service.CreateCereal(createdCereal));
    }

    [TestMethod]
    public void CreateCereals_ValidCerealData_AddsCereals()
    {
        // Arrange
        var newCereals = new List<CerealData>
        {
            new() { Id = 0, Name = "A" },
            new() { Id = 15, Name = "B" },
            new() { Id = 30, Name = "C" },
        };

        using var context = FakeDbContext.Create();
        context.SaveChanges();

        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        var service = new CerealService(context, httpContextAccessor.Object);

        // Act
        service.CreateCereals(newCereals);
        DateTime dbVersion = DateTime.UtcNow;
        var all = service.GetAllCereals();

        // Assert
        Assert.IsNotNull(all);
        Assert.HasCount(newCereals.Count, all);
        for (int i = 0; i < all.Count; i++)
        {
            Assert.AreEqual(newCereals[i].Name, all[i].Name);
            Assert.AreEqual(i + 1, all[i].Id);
            Assert.AreEqual(dbVersion.Date, all[i].DbVersion?.Date);
        }
    }

    [TestMethod]
    [DataRow("", (CerealEnums.Manufacturer)0, (CerealEnums.Type)0, 100)]    // Invalid Name
    [DataRow("B", (CerealEnums.Manufacturer)999, (CerealEnums.Type)0, 100)] // Invalid Manufacturer
    [DataRow("B", (CerealEnums.Manufacturer)0, (CerealEnums.Type)999, 100)] // Invalid Type
    [DataRow("B", (CerealEnums.Manufacturer)0, (CerealEnums.Type)0, -1)]    // Invalid Number
    public void CreateCereals_FailingValidation_ThrowsArgumentException(string name, CerealEnums.Manufacturer manufacturer, CerealEnums.Type type, int calories)
    {
        // Arrange
        var newCereals = new List<CerealData>
        {
            new() { Id = 0, Name = "A" },
            new() { Id = 15, Name = name, Manufacturer = manufacturer, Type = type, Calories = calories },
            new() { Id = 30, Name = "C" },
        };

        using var context = FakeDbContext.Create();
        context.SaveChanges();

        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        var service = new CerealService(context, httpContextAccessor.Object);

        // Act & Assert
        Assert.ThrowsExactly<ArgumentException>(() => service.CreateCereals(newCereals));
    }

    [TestMethod]
    public void UpdateCereal_ExistingId_UpdatesValues()
    {
        // Arrange
        int id = 1;
        string oldName = "Old";
        DateTime dbVersion = DateTime.UtcNow.AddDays(-5);

        using var context = FakeDbContext.Create();
        context.Add(new CerealData { Id = id, Name = oldName, DbVersion = dbVersion });
        context.SaveChanges();

        string updatedName = "Updated";
        UpdateCerealDTO updatedCereal = new() { Id = id, Name = updatedName, DbVersion = dbVersion };

        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        var service = new CerealService(context, httpContextAccessor.Object);

        // Act
        var actual = service.UpdateCereal(updatedCereal);

        // Assert
        Assert.IsNotNull(actual);
        Assert.AreEqual(id, actual.Id);
        Assert.AreEqual(updatedName, actual.Name);
        Assert.AreNotEqual(dbVersion.Date, actual.DbVersion?.Date);
        Assert.AreNotEqual(oldName, actual.Name);
    }

    [TestMethod]
    public void UpdateCereal_InvalidId_ThrowsKeyNotFound()
    {
        // Arrange
        UpdateCerealDTO updatedCereal = new() { Id = 1, Name = "Updated" };

        using var context = FakeDbContext.Create();
        context.SaveChanges();

        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        var service = new CerealService(context, httpContextAccessor.Object);

        // Act & Assert
        Assert.ThrowsExactly<KeyNotFoundException>(() => service.UpdateCereal(updatedCereal));
    }

    [TestMethod]
    public void UpdateCereal_ConcurrentUpdate_ReturnsInvalidOperationException()
    {
        // Arrange
        int id = 1;
        DateTime dbVersion = DateTime.UtcNow.AddDays(-5);

        UpdateCerealDTO updatedCereal = new() { Id = id, Name = "Updated", DbVersion = dbVersion };

        using var context = FakeDbContext.Create();
        context.Add(new CerealData { Id = id, Name = "Old", DbVersion = dbVersion.AddDays(2) });
        context.SaveChanges();

        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        var service = new CerealService(context, httpContextAccessor.Object);

        // Act & Assert
        Assert.ThrowsExactly<InvalidOperationException>(() => service.UpdateCereal(updatedCereal));
    }

    [TestMethod]
    public void DeleteCereal_ExistingId_MarksCerealAsDeleted()
    {
        // Arrange
        int idToDelete = 2;
        DateTime dbVersion = DateTime.UtcNow.AddDays(-5);

        using var context = FakeDbContext.Create();
        context.AddRange(
            new CerealData { Id = 1, Name = "A", DbVersion = dbVersion },
            new CerealData { Id = 2, Name = "B", DbVersion = dbVersion },
            new CerealData { Id = 3, Name = "C", DbVersion = dbVersion }
            );
        context.SaveChanges();

        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        var service = new CerealService(context, httpContextAccessor.Object);

        // Act
        var actual = service.DeleteCereal(idToDelete);

        // Assert
        Assert.IsNotNull(actual);
        Assert.AreEqual(idToDelete, actual.Id);
        Assert.IsTrue(actual.MarkedAsDeleted);
        Assert.AreNotEqual(dbVersion.Date, actual.DbVersion?.Date);
    }

    [TestMethod]
    public void DeleteCereal_InvalidId_ThrowsKeyNotFound()
    {
        // Arrange
        using var context = FakeDbContext.Create();
        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        var service = new CerealService(context, httpContextAccessor.Object);

        // Act & Assert
        Assert.ThrowsExactly<KeyNotFoundException>(() => service.DeleteCereal(1));
    }

    [TestMethod]
    public void RestoreCereal_ExistingId_RemovesMarkedAsDelete()
    {
        // Arrange
        int deletedId = 2;
        DateTime dbVersion = DateTime.UtcNow.AddDays(-5);

        using var context = FakeDbContext.Create();
        context.AddRange(
            new CerealData { Id = 1, Name = "A", DbVersion = dbVersion },
            new CerealData { Id = 2, Name = "B", DbVersion = dbVersion, MarkedAsDeleted = true },
            new CerealData { Id = 3, Name = "C", DbVersion = dbVersion }
            );
        context.SaveChanges();

        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        var service = new CerealService(context, httpContextAccessor.Object);

        // Act
        var actual = service.RestoreCereal(deletedId);

        // Assert
        Assert.IsNotNull(actual);
        Assert.AreEqual(deletedId, actual.Id);
        Assert.IsFalse(actual.MarkedAsDeleted);
        Assert.AreNotEqual(dbVersion.Date, actual.DbVersion?.Date);
    }

    [TestMethod]
    public void RestoreCereal_InvalidId_ThrowsKeyNotFound()
    {
        // Arrange
        using var context = FakeDbContext.Create();
        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        var service = new CerealService(context, httpContextAccessor.Object);

        // Act & Assert
        Assert.ThrowsExactly<KeyNotFoundException>(() => service.RestoreCereal(1));
    }
}
