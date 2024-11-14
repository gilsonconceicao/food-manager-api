using FoodManager.API.Controllers;
using FoodManager.API.Services;
using FoodManager.Application.Carts.Commands;
using FoodManager.Application.Carts.Dtos;
using FoodManager.Application.Carts.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodManager.API.Controllers;

public class CartController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IHttpUserService _tokenService;

    public CartController(
        IMediator mediator,
        IHttpUserService tokenService)
    {
        _mediator = mediator;
        _tokenService = tokenService;
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
    /// Adiciona um novo item no carrinho
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "Auth")]
    public async Task<ActionResult> AddCartAsync([FromBody] CartCreateCommand request)
    {
        var user = await _tokenService.VerifyTokenAsync();
        var result = await _mediator.Send(new CartCreateCommand
        {
            ItemId = request.ItemId,
            Quantity = request.Quantity,
            UserId = user.UserId
        });
        return Ok(result);
    }

    /// <summary>
    /// Atualiza um item no carrinho
    /// </summary>
    [HttpPut("{CartId}")]
    [Authorize(Policy = "Auth")]
    public async Task<ActionResult> UpdateCartAsync([FromRoute] Guid CartId, [FromBody] CartCreateDto request)
    {
        var result = await _mediator.Send(new CreateUpdateCommand
        {
            CartId = CartId,
            Quantity = request.Quantity,
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