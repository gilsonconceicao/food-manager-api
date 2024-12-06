using AutoMapper;
using Api.Services;
using Application.Users.Commands;
using Application.Users.Dtos;
using Application.Users.Queries;
using Domain.Extensions;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
using Application.Payment.Commands;

namespace Api.Controllers;

public class PaymentController : BaseController
{
    private readonly IMediator _mediator;

    public PaymentController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Método para obter um pedido através do identificador 
    /// </summary>
    /// <response code="200">200 Sucesso</response>
    /// <response code="400">400 Erro</response>
    [ProducesResponseType<string>(StatusCodes.Status201Created)]
    [HttpPost]
    [Authorize(Policy = "Auth")]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentCommand createPaymentCommand)
    {
        var result = await _mediator.Send(createPaymentCommand);
        return Ok(result);
    }
}