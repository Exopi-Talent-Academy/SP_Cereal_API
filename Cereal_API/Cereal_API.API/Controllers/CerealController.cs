using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cereal_API.API.Controllers;

[ApiController]
[Route("api/cereal")]
public class CerealController(ICerealService cerealService) : ControllerBase
{
    [Authorize]
    [HttpGet("auth/check", Name = "AuthorizationCheck")]
    public IActionResult AuthorizationCheck()
    {
        return Ok(new AuthenticationResult
        {
            Username = User.Identity?.Name ?? "Unknown",
            Role = User.FindFirst(ClaimTypes.Role)?.Value ?? "Unknown",
        });
    }

    [Authorize]
    [HttpGet(Name = "GetAllCereals")]
    public IActionResult GetAll()
    {
        return Ok(cerealService.GetAllCereals());
    }

    [Authorize]
    [HttpGet("{id}", Name = "GetCereal")]
    public IActionResult Get(int id)
    {
        try
        {
            return Ok(cerealService.GetCereal(id));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [Authorize]
    [HttpGet("{id}/image")]
    public IActionResult GetImage(int id)
    {
        try
        {
            var (Data, ContentType) = cerealService.GetImage(id);
            return File(Data, ContentType);
        }
        catch (FileNotFoundException)
        {
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [Authorize(Roles = "Editor,Admin")]
    [HttpPost(Name = "CreateCereal")]
    public IActionResult Create(CreateCerealDTO data)
    {
        try
        {
            return Ok(cerealService.CreateCereal(data));
        }
        catch
        {
            return BadRequest();
        }
    }

    [Authorize(Roles = "Editor,Admin")]
    [HttpPost("bulk", Name = "CreateCereals")]
    public IActionResult CreateBulk(IFormFile file)
    {
        if (file == null)
            return BadRequest();

        List<ErrorReport> errorReports = [];
        try
        {
            cerealService.CreateCereals(CsvImporter.Load(file, errorReports));
        }
        catch (Exception ex)
        {
            errorReports.Add(new("Bulk import failed", ex.Message));
        }

        return Ok(errorReports);
    }

    [Authorize(Roles = "Editor,Admin")]
    [HttpPut(Name = "UpdateCereal")]
    public IActionResult Update(UpdateCerealDTO data)
    {
        try
        {
            return Ok(cerealService.UpdateCereal(data));
        }
        catch (InvalidOperationException)
        {
            return Conflict();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}", Name = "DeleteCereal")]
    public IActionResult Delete(int id)
    {
        try
        {
            return Ok(cerealService.DeleteCereal(id));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id}/restore", Name = "RestoreCereal")]
    public IActionResult Restore(int id)
    {
        try
        {
            return Ok(cerealService.RestoreCereal(id));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
