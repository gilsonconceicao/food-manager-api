using FoodManager.Application.Foods.Commands.CreateFoodCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodManager.API.Controllers;
public class FoodController : BaseController
{
    private readonly IMediator _mediator ;
    public FoodController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost]
    public async Task<IActionResult> CreateFoodAsync([FromBody] CreateFoodCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
