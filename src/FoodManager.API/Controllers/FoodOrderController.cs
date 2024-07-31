using FoodManager.Application.FoodsOrders.Commands.OrderCreateCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodManager.API.Controllers;

public class FoodOrderController : BaseController
{
    private readonly IMediator _mediator;
    public FoodOrderController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Método utilizado para adicionar comida
    /// </summary>
    /// <returns>Food</returns>
    /// <response code="200">200 Sucesso</response>
    /// <response code="400">400 Erro</response>
    [ProducesResponseType<bool>(StatusCodes.Status200OK)]
    [HttpPost]
    public async Task<IActionResult> CreateFoodOrder([FromBody] OrderCreateCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}