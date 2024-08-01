using AutoMapper;
using FoodManager.Application.Orders.Commands.OrderCreateCommand;
using FoodManager.Application.Orders.Dtos;
using FoodManager.Application.Orders.Queries.OrderGetByIdQuery;
using FoodManager.Application.Orders.Queries.OrderPaginationListQuery;
using FoodManager.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodManager.API.Controllers;

public class OrderController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public OrderController(IMediator mediator,
    IMapper mapper)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Método utilizado para adicionar comida
    /// </summary>
    /// <returns>Order</returns>
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
    /// <returns>Order</returns>
    /// <response code="200">200 Sucesso</response>
    /// <response code="400">400 Erro</response>
    [ProducesResponseType<List<OrderGetDto>>(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<IActionResult> OrderGetListAsync([FromQuery] OrderPaginationListQuery query)
    {
        var result = await _mediator.Send(query);
        var listMapped = _mapper.Map<List<OrderGetDto>>(result.Data);

        var listPaginated = new PagedList<OrderGetDto>(
            data: listMapped,
            count: result.Count ?? 0,
            pageNumber: query.Page,
            pageSize: query.Size
        );
        
        return Ok(listPaginated);
    }

    /// <summary>
    /// Método para obter um pedido através do identificador 
    /// </summary>
    /// <returns>Order</returns>
    /// <response code="200">200 Sucesso</response>
    /// <response code="400">400 Erro</response>
    [ProducesResponseType<OrderGetDto>(StatusCodes.Status200OK)]
    [HttpGet("{Id}")]
    public async Task<IActionResult> OrderGetByIdAsync(Guid Id)
    {
        var result = await _mediator.Send(new OrderGetByIdQuery(Id));
        var projectedData = _mapper.Map<OrderGetDto>(result);
        return Ok(projectedData);
    }
}