using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodManager.API.Controllers;
public class FoodController : BaseController
{
    private readonly IMediator _mediator;
    public FoodController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return Ok();
    }
}
