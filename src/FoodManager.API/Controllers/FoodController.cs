using FoodManager.Application.Foods.Commands;
using FoodManager.Application.Foods.Commands.Dtos;
using FoodManager.Application.Foods.Queries.GetAllWithPaginationFoodQuery;
using FoodManager.Application.Foods.Queries.GetFoodByIdQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using FoodManager.Domain.Extensions;

namespace FoodManager.API.Controllers;
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
    [ProducesResponseType<PagedList<GetFoodDto>>(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<IActionResult> GetAllFoodsWithPaginationAsync([FromQuery] GetAllWithPaginationFoodQuery query)
    {
        var result = await _mediator.Send(query);
        
        var foodList = _mapper.Map<List<GetFoodDto>>(result.Data);

        var listMappedFromPagination = new PagedList<GetFoodDto>(
            data: foodList, 
            count: result.Count ?? 0, 
            pageNumber: query.Page, 
            pageSize: query.Size
        );

        return Ok(listMappedFromPagination);
    }

    /// <summary>
    /// Obtem um regitro de comida por identifcador
    /// </summary>
    [ProducesResponseType<GetFoodDto>(StatusCodes.Status200OK)]
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetFoodByIdAsync(Guid Id)
    {
        var result = await _mediator.Send(new GetFoodByIdQuery(Id));
        return Ok(_mapper.Map<GetFoodDto>(result));
    }

    /// <summary>
    /// Remove um registro de comida por identifcador 
    /// </summary>
    [ProducesResponseType<bool>(StatusCodes.Status204NoContent)]
    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteFoodAsync(Guid Id)
    {
        var result = await _mediator.Send(new FoodDeleteCommand(Id));
        return Ok(result);
    }

    /// <summary>
    /// Atualiza um registro de comida por identifcador 
    /// </summary>
    [ProducesResponseType<bool>(StatusCodes.Status204NoContent)]
    [HttpPut("{Id}")]
    public async Task<IActionResult> UpdateFoodAsync(Guid Id, [FromBody] FoodUpdateDto model)
    {
        var result = await _mediator.Send(new FoodUpdateCommand
        (
            Id: Id,
            Category: model.Category,
            Description: model.Description,
            IsAvailable: model.IsAvailable,
            Name: model.Name,
            PreparationTime: model.PreparationTime,
            Price: model.Price,
            UrlImage: model.UrlImage
        ));

        return Ok(result);
    }
}
