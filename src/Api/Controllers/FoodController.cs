using Application.Foods.Commands;
using Application.Foods.Commands.Dtos;
using Application.Foods.Queries.GetAllWithPaginationFoodQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Domain.Extensions;
using Application.Foods.Queries.FoodGetListPaginationQuery;
using Microsoft.AspNetCore.Authorization;
using Application.Foods.Queries.FoodGetByIdQuery;

namespace Api.Controllers;
public class FoodController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    public FoodController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Método utilizado para adicionar comida
    /// </summary>
    [ProducesResponseType<List<bool>>(StatusCodes.Status201Created)]
    [HttpPost]
    public async Task<IActionResult> CreateFoodAsync([FromBody] FoodCreateCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Consulta todas as comidas com paginação
    /// </summary>
    [ProducesResponseType<PagedList<FoodDto>>(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<IActionResult> GetAllFoodsWithPaginationAsync([FromQuery] FoodGetListPaginationQuery query)
    {
        var result = await _mediator.Send(query);

        var foodList = _mapper.Map<List<FoodDto>>(result.Data);

        var listMappedFromPagination = new PagedList<FoodDto>(
            data: foodList,
            count: result.Count ?? 0,
            pageNumber: query.Page,
            pageSize: query.Size
        );

        return Ok(listMappedFromPagination);
    }

    /// <summary>
    /// Obtem comida por identificador
    /// </summary>
    [ProducesResponseType<FoodDto>(StatusCodes.Status200OK)]
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetFoodById([FromRoute] Guid Id)
    {
        var result = await _mediator.Send(new FoodGetByIdQuery
        { 
            Id = Id
        });

        var resultMapped = _mapper.Map<FoodDto>(result);

        return Ok(resultMapped);
    }

    /// <summary>
    /// Remove um registro de comida por identifcador 
    /// </summary>
    [ProducesResponseType<bool>(StatusCodes.Status204NoContent)]
    [HttpDelete("{Id}")]
    [Authorize(Policy = "Auth")]
    public async Task<IActionResult> DeleteFoodAsync(Guid Id)
    {
        var result = await _mediator.Send(new FoodDeleteCommand(Id));
        return Ok(result);
    }

    /// <summary>
    /// Atualiza um registro de comida por identifcador 
    /// </summary>
    [ProducesResponseType<bool>(StatusCodes.Status204NoContent)]
    [HttpPatch("{Id}")]
    [Authorize(Policy = "Auth")] 

    public async Task<IActionResult> UpdateFoodAsync(Guid Id, [FromBody] FoodUpdateDto model)
    {
        var result = await _mediator.Send(new FoodUpdateCommand
        (
            Id: Id,
            Category: model.Category,
            Description: model.Description,
            IsAvailable: model.IsAvailable,
            Name: model.Name,
            Price: model.Price,
            Url: model.Url
        ));

        return Ok(result);
    }
}
