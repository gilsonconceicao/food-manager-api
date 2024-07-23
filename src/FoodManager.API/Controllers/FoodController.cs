using Microsoft.AspNetCore.Mvc;

namespace FoodManager.API.Controllers;
public class FoodController : BaseController
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok();
    }
}
