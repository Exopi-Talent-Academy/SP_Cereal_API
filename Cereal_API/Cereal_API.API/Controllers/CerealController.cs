using Cereal_API.API.Services;
using Cereal_API.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cereal_API.API.Controllers;

[ApiController]
[Route("api/cereal")]
public class CerealController(ICerealService cerealService) : ControllerBase
{
    [Authorize]
    [HttpGet("auth/check", Name = "AuthorizationCheck")]
    public IActionResult AuthorizationCheck()
    {
        return Ok();
    }

    [HttpGet(Name = "GetAllCereals")]
    public IActionResult GetAll()
    {
        return Ok(cerealService.GetAllCereals());
    }

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

    [Authorize]
    [HttpPost(Name = "CreateCereal")]
    public IActionResult Create(CerealData data)
    {
        try
        {
            cerealService.CreateCereal(data);
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }

    [Authorize]
    [HttpPut(Name = "UpdateCereal")]
    public IActionResult Update(CerealData data)
    {
        try
        {
            cerealService.UpdateCereal(data);
            return Ok();
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

    [Authorize]
    [HttpDelete("{id}", Name = "DeleteCereal")]
    public IActionResult Delete(int id)
    {
        try
        {
            cerealService.DeleteCereal(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
