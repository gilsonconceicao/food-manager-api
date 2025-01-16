using Api.Controllers;
using Api.Services;
using Application.Carts.Commands;
using Application.Carts.Dtos;
using Application.Carts.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class CartController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CartController(
        IMediator mediator,
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtem todos os itens do carrinho
    /// </summary>
    [HttpGet]
    [Authorize(Policy = "Auth")]
    public async Task<ActionResult> GetAllAsync()
    {
        var result = await _mediator.Send(new CartGetListQuery { });
        return Ok(result);
    }

    /// <summary>
    /// Adiciona ou atualiza um novo item no carrinho
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "Auth")]
    public async Task<ActionResult> AddCartAsync([FromBody] CartCreateCommand request)
    {
        var result = await _mediator.Send(new CartCreateCommand
        {
            ItemId = request.ItemId,
            Quantity = request.Quantity
        });
        return Ok(result);
    }

    /// <summary>
    /// Remove um item no carrinho
    /// </summary>
    [HttpDelete("{CartId}")]
    [Authorize(Policy = "Auth")]
    public async Task<ActionResult> DeleteCartAsync([FromRoute] Guid CartId)
    {
        var result = await _mediator.Send(new CartDeleteCommand
        {
            CartId = CartId
        });

        return Ok(result);
    }
}