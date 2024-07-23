using Microsoft.AspNetCore.Mvc;

namespace FoodManager.API.Controllers;
[ApiController]
[Route("api/[Controller]")]
public class FoodController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok();
    }
}
