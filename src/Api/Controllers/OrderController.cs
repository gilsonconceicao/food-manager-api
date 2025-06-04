using AutoMapper;
using Application.Orders.Dtos;
using Application.Orders.Commands;
using Application.Orders.Queries;
using Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers;

public class OrderController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public OrderController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    }

    /// <summary>
    /// Método utilizado para criar um pedido
    /// </summary>
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<bool>(StatusCodes.Status201Created)]
    [HttpPost("{UserId}")]
    [Authorize(Policy = "Auth")]
    public async Task<IActionResult> OrderCreateAsync([FromRoute] string UserId, [FromBody] OrderCreateDto model)
    {
        var result = await _mediator.Send(new OrderCreateCommand
        {
            UserId = UserId,
            CartIds = model.CartIds
        });
        return Ok(result);
    }

    /// <summary>
    /// Método para obter a lista de pedidos
    /// </summary>
    /// <response code="200">200 Sucesso</response>
    /// <response code="400">400 Erro</response>
    [ProducesResponseType<List<OrderDto>>(StatusCodes.Status200OK)]
    [HttpGet]
    [Authorize(Policy = "Auth")]
    public async Task<IActionResult> OrderGetListAsync([FromQuery] OrderPaginationListQuery query)
    {
        var result = await _mediator.Send(query);
        var listMapped = _mapper.Map<List<OrderDto>>(result.Data);

        var listPaginated = new PagedList<OrderDto>(
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
    /// <response code="200">200 Sucesso</response>
    /// <response code="400">400 Erro</response>
    [ProducesResponseType<OrderDto>(StatusCodes.Status200OK)]
    [HttpGet("{Id}")]
    [Authorize(Policy = "Auth")]
    public async Task<IActionResult> OrderGetByIdAsync(Guid Id)
    {
        var result = await _mediator.Send(new OrderGetByIdQuery(Id));
        var projectedData = _mapper.Map<OrderDto>(result);
        return Ok(projectedData);
    }

    /// <summary>
    /// Método para remover um pedido da lista
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="IsPermanent"></param>
    /// <response code="200">200 Sucesso</response>
    /// <response code="400">400 Erro</response>
    [ProducesResponseType<bool>(StatusCodes.Status204NoContent)]
    [HttpDelete("{Id}")]
    [Authorize(Policy = "Auth")]
    public async Task<IActionResult> OrderDeleteByIdAsync(Guid Id, [FromQuery] bool IsPermanent)
    {
        var result = await _mediator.Send(new OrderDeleteCommand
        {
            OrderId = Id,
            IsPermanent = IsPermanent
        });
        return Ok(result);
    }

    /// <summary>
    /// Método para cancelar um pedido da lista
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="IsPermanent"></param>
    /// <response code="200">200 Sucesso</response>
    /// <response code="400">400 Erro</response>
    [ProducesResponseType<bool>(StatusCodes.Status204NoContent)]
    [HttpPut("{Id}/Cancel")]
    [Authorize(Policy = "Auth")]
    public async Task<IActionResult> OrderCancelledByIdAsync(Guid Id, [FromQuery] bool IsPermanent)
    {
        var result = await _mediator.Send(new OrderCancelCommand
        {
            OrderId = Id
        });
        return Ok(result);
    }
}