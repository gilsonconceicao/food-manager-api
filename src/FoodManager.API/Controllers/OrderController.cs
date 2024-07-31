using FoodManager.Application.Orders.Commands.OrderCreateCommand;
using FoodManager.Application.Orders.Queries.OrderPaginationListQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodManager.API.Controllers;

public class OrderController : BaseController
{
    private readonly IMediator _mediator;
    public OrderController(IMediator mediator)
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
    public async Task<IActionResult> OrderCreateAsync([FromBody] OrderCreateCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Método para obter a lista de pedidos
    /// </summary>
    /// <returns>Food</returns>
    /// <response code="200">200 Sucesso</response>
    /// <response code="400">400 Erro</response>
    [ProducesResponseType<bool>(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<IActionResult> OrderGetListAsync([FromQuery] OrderPaginationListQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}