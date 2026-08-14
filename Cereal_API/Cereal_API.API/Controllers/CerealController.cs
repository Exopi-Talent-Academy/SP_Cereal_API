using Cereal_API.API.Services;
using Cereal_API.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cereal_API.API.Controllers;

[ApiController]
[Route("api/cereal")]
public class CerealController(ICerealService cerealService) : ControllerBase
{
    [HttpGet(Name = "GetAllCereals")]
    public IEnumerable<CerealData[]> GetAll()
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}", Name = "GetCereal")]
    public CerealData Get(int id)
    {
        //throw new NotImplementedException();
        return cerealService.GetCereal(id);
    }

    [HttpGet("{id}/image")]
    public IActionResult GetImage(int id)   // Unsure about the return type here
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
