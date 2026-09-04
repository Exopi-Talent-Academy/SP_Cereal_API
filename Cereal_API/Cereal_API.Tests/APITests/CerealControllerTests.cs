using Cereal_API.API.Controllers;
using Cereal_API.API.Services;
using Cereal_API.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace Cereal_API.Tests.APITests;

[TestClass]
public class CerealControllerTests
{
    [TestMethod]
    public void AuthorizationCheck_ReturnsOK()
    {
        // Arrange
        var service = new Mock<ICerealService>();
        var controller = new CerealController(service.Object);

        var claims = new[] 
        { 
            new Claim(ClaimTypes.Name, "admin"), 
            new Claim(ClaimTypes.Role, "Admin") 
        }; 
        
        var identity = new ClaimsIdentity(claims, "Basic"); 
        
        controller.ControllerContext = new ControllerContext 
        { 
            HttpContext = new DefaultHttpContext 
            { 
                User = new ClaimsPrincipal(identity) 
            } 
        };

        // Act
        var actual = controller.AuthorizationCheck();

        // Assert
        Assert.IsInstanceOfType<OkObjectResult>(actual);
    }

    [TestMethod]
    public void GetAll_ReturnsOkWithCereals()
    {
        // Arrange
        var service = new Mock<ICerealService>();
        service.Setup(s => s.GetAllCereals()).Returns(
            [
                new CerealData { Id = 1, Name = "A" },
                new CerealData { Id = 2, Name = "B" },
                new CerealData { Id = 3, Name = "C" }
            ]);

        var controller = new CerealController(service.Object);

        // Act
        var actual = controller.GetAll();

        // Assert
        Assert.IsInstanceOfType<OkObjectResult>(actual);
        var cereals = (actual as OkObjectResult)?.Value as List<CerealData>;
        Assert.IsNotNull(cereals);
        Assert.HasCount(3, cereals);
    }

    [TestMethod]
    public void Get_ValidId_ReturnsOK()
    {
        // Arrange
        int expectedId = 1;

        var service = new Mock<ICerealService>();
        service.Setup(s => s.GetCereal(expectedId)).Returns(new CerealData { Id = expectedId, Name = "A" });

        var controller = new CerealController(service.Object);

        // Act
        var actual = controller.Get(expectedId);

        // Assert
        Assert.IsInstanceOfType<OkObjectResult>(actual);
    }

    [TestMethod]
    public void Get_InvalidId_ReturnsNotFound()
    {
        // Arrange
        int id = 1;

        var service = new Mock<ICerealService>();
        service.Setup(s => s.GetCereal(id)).Throws<KeyNotFoundException>();

        var controller = new CerealController(service.Object);

        // Act
        var actual = controller.Get(id);

        // Assert
        Assert.IsInstanceOfType<NotFoundResult>(actual);
    }

    [TestMethod]
    public void GetImage_ValidId_ReturnsFileContent()
    {
        // Arrange
        int id = 1;
        byte[] mockImage = [1, 2, 3, 4, 5];
        string mockContentType = "image/jpeg";

        var service = new Mock<ICerealService>();
        service.Setup(s => s.GetImage(id)).Returns((mockImage, mockContentType));

        var controller = new CerealController(service.Object);

        // Act
        var actual = controller.GetImage(id);

        // Assert
        Assert.IsInstanceOfType<FileContentResult>(actual);

        var image = (actual as FileContentResult)?.FileContents;
        Assert.IsNotNull(image);
        Assert.HasCount(5, image);
        Assert.AreEqual(mockImage, image);

        var contentType = (actual as FileContentResult)?.ContentType;
        Assert.IsNotNull(contentType);
        Assert.AreEqual(mockContentType, contentType);
    }

    [TestMethod]
    public void GetImage_ValidIdButMissingImage_ReturnNoContent()
    {
        // Arrange
        int id = 1;

        var service = new Mock<ICerealService>();
        service.Setup(s => s.GetImage(id)).Throws<FileNotFoundException>();

        var controller = new CerealController(service.Object);

        // Act
        var actual = controller.GetImage(id);

        // Assert
        Assert.IsInstanceOfType<NoContentResult>(actual);
    }

    [TestMethod]
    public void GetImage_InvalidId_ReturnNotFound()
    {
        // Arrange
        int id = 1;

        var service = new Mock<ICerealService>();
        service.Setup(s => s.GetImage(id)).Throws<KeyNotFoundException>();

        var controller = new CerealController(service.Object);

        // Act
        var actual = controller.GetImage(id);

        // Assert
        Assert.IsInstanceOfType<NotFoundResult>(actual);
    }

    [TestMethod]
    public void Create_ValidRequest_ReturnsOK()
    {
        // Arrange
        CerealData newCereal = new() { Id = 0, Name = "New" };

        var service = new Mock<ICerealService>();
        var controller = new CerealController(service.Object);

        // Act
        var actual = controller.Create(newCereal);

        // Assert
        Assert.IsInstanceOfType<OkResult>(actual);
        service.Verify(x => x.CreateCereal(newCereal), Times.Once);
    }

    [TestMethod]
    public void Create_InvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        CerealData newCereal = new() { Id = 0, Name = "New" };

        var service = new Mock<ICerealService>();
        service.Setup(s => s.CreateCereal(newCereal)).Throws<ArgumentException>();

        var controller = new CerealController(service.Object);

        // Act
        var actual = controller.Create(newCereal);

        // Assert
        Assert.IsInstanceOfType<BadRequestResult>(actual);
    }

    [TestMethod]
    public void CreateBulk_ValidRequest_ReturnsOKObject()
    {
        // Arrange
        var newCereals = new List<CerealData>
        {
            new() { Id = 0, Name = "A" },
            new() { Id = 15, Name = "B" },
            new() { Id = 30, Name = "C" }
        };
        var mockFile = new Mock<IFormFile>();
        // This doesn't work below, so maybe try to use an actual test file!
        mockFile.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(System.Text.Encoding.UTF8.GetBytes("Id,Name,Manufacturer,Type,Calories,Protein,Fat,Sodium,Fiber,Carbo,Sugars,Potass,Vitamins,Shelf,Weight,Cups,Rating,DbVersion,\n0,A\n15,B\n30,C")));
        mockFile.Setup(f => f.FileName).Returns("mock.csv");
        mockFile.Setup(f => f.Length).Returns(100);

        var service = new Mock<ICerealService>();
        var controller = new CerealController(service.Object);

        // Act
        var actual = controller.CreateBulk(mockFile.Object);

        // Assert
        Assert.IsInstanceOfType<OkObjectResult>(actual);
        service.Verify(x => x.CreateCereals(newCereals), Times.Once);
    }

    [TestMethod]
    public void Update_ValidRequest_ReturnsOK()
    {
        // Arrange
        CerealData newCereal = new() { Id = 1, Name = "New" };

        var service = new Mock<ICerealService>();
        var controller = new CerealController(service.Object);

        // Act
        var actual = controller.Update(newCereal);

        // Assert
        Assert.IsInstanceOfType<OkResult>(actual);
        service.Verify(x => x.UpdateCereal(newCereal), Times.Once);
    }

    [TestMethod]
    public void Update_InvalidId_ReturnNotFound()
    {
        // Arrange
        CerealData newCereal = new() { Id = 1, Name = "New" };

        var service = new Mock<ICerealService>();
        service.Setup(s => s.UpdateCereal(newCereal)).Throws<KeyNotFoundException>();

        var controller = new CerealController(service.Object);

        // Act
        var actual = controller.Update(newCereal);

        // Assert
        Assert.IsInstanceOfType<NotFoundResult>(actual);
    }

    [TestMethod]
    public void Update_ConcurrentUpdate_ReturnsConflict()
    {
        // Arrange
        CerealData newCereal = new() { Id = 1, Name = "New" };

        var service = new Mock<ICerealService>();
        service.Setup(s => s.UpdateCereal(newCereal)).Throws<InvalidOperationException>();

        var controller = new CerealController(service.Object);

        // Act
        var actual = controller.Update(newCereal);

        // Assert
        Assert.IsInstanceOfType<ConflictResult>(actual);
    }

    [TestMethod]
    public void Delete_ExistingId_ReturnsNoContent()
    {
        // Arrange
        int id = 1;

        var service = new Mock<ICerealService>();
        var controller = new CerealController(service.Object);

        // Act
        var actual = controller.Delete(id);

        // Assert
        Assert.IsInstanceOfType<NoContentResult>(actual);
        service.Verify(x => x.DeleteCereal(id), Times.Once);
    }

    [TestMethod]
    public void Delete_InvalidId_ReturnNotFound()
    {
        // Arrange
        int id = 1;

        var service = new Mock<ICerealService>();
        service.Setup(s => s.DeleteCereal(id)).Throws<KeyNotFoundException>();

        var controller = new CerealController(service.Object);

        // Act
        var actual = controller.Delete(id);

        // Assert
        Assert.IsInstanceOfType<NotFoundResult>(actual);
    }
}
