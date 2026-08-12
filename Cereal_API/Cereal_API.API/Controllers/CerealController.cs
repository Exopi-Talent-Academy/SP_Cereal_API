using Cereal_API.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cereal_API.API.Controllers;

[ApiController]
[Route("api/cereal")]
public class CerealController : ControllerBase
{
    [HttpGet]
    public IEnumerable<CerealData[]> Get()
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}")]
    public IEnumerable<CerealData> Get(int id)
    {
        throw new NotImplementedException();
    }

    /*[HttpGet] // Maybe make this in another Controller?
    public string GetImage()    // Consider the return type of the image (path or actual file?) and how to aquire it
    {
        throw new NotImplementedException();
    }*/

    [HttpPost()]
    public IActionResult Update(CerealData data)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{id}")]
    public IActionResult Add(int id)
    {
        throw new NotImplementedException(); 
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        throw new NotImplementedException();
    }
}
