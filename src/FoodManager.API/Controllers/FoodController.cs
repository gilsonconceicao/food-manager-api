using FoodManager.Application.Foods.Commands.CreateFoodCommand;
using FoodManager.Application.Foods.Commands.DeleteFoodCommand;
using FoodManager.Application.Foods.Queries.GetAllWithPaginationFoodQuery;
using FoodManager.Application.Foods.Queries.GetFoodByIdQuery;
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
    public async Task<IActionResult> GetAllFoodsWithPaginationAsync([FromQuery] GetAllWithPaginationFoodQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Obtem um regitro de comida por identifcador
    /// </summary>
    /// <returns>Food</returns>
    /// <response code="200">200 Sucesso</response>
    /// <response code="400">400 Erro</response>
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetFoodByIdAsync(Guid Id)
    {
        var result = await _mediator.Send(new GetFoodByIdQuery(Id));
        return Ok(result);
    }

    /// <summary>
    /// Remove um registro de comida por identifcador 
    /// </summary>
    /// <returns>Food</returns>
    /// <response code="200">200 Sucesso</response>
    /// <response code="400">400 Erro</response>
    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteFoodAsync(Guid Id)
    {
        var result = await _mediator.Send(new DeleteFoodCommand(Id));
        return Ok(result);
    }
}
