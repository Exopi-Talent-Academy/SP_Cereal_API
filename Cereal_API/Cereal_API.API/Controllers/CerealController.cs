using Cereal_API.API.Services;
using Cereal_API.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cereal_API.API.Controllers;

[ApiController]
[Route("api/cereal")]
public class CerealController(ICerealService cerealService) : ControllerBase
{
    [HttpGet(Name = "GetAllCereals")]
    public IActionResult GetAll()
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}", Name = "GetCereal")]
    public IActionResult Get(int id)
    {
        //throw new NotImplementedException();
        return cerealService.GetCereal(id);
    }

    [HttpGet("{id}/image")]
    public IActionResult GetImage(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost(Name = "CreateCereal")]
    public IActionResult Create(CerealData data)
    {
        throw new NotImplementedException(); 
    }

    [HttpPut(Name = "UpdateCereal")]
    public IActionResult Update(CerealData data)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id}", Name = "DeleteCereal")]
    public IActionResult Delete(int id)
    {
        throw new NotImplementedException();
    }
}
