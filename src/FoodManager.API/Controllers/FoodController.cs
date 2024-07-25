using FoodManager.Application.Foods.Commands.CreateFoodCommand;
using FoodManager.Application.Foods.Queries.GetAllWithPaginationFoodQuery;
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

    /// <summary>
    /// Método utilizado para adicionar comida
    /// </summary>
    /// <returns>Food</returns>
    /// <response code="200">200 Sucesso</response>
    /// <response code="400">400 Erro</response>
    [HttpPost]
    public async Task<IActionResult> CreateFoodAsync([FromBody] CreateFoodCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Consulta todas as comidas com paginação
    /// </summary>
    /// <returns>Food</returns>
    /// <response code="200">200 Sucesso</response>
    /// <response code="400">400 Erro</response>
    [HttpGet]
    public async Task<IActionResult> GetAllFoodsWithPagination([FromQuery] GetAllWithPaginationFoodQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
