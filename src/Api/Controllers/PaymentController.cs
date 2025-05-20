using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    /// Método para gerar um pagamento 
    /// </summary>
    /// <response code="200">200 Sucesso</response>
    /// <response code="400">400 Erro</response>
    [ProducesResponseType<string>(StatusCodes.Status201Created)]
    [HttpPost("CreatePreference")]
    [Authorize(Policy = "Auth")]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentCommand createPaymentCommand)
    {
        var result = await _mediator.Send(createPaymentCommand);
        return Ok(result);
    }

    /// <summary>
    /// Obtem uma preferência de pagamento
    /// </summary>
    /// <response code="200">200 Sucesso</response>
    /// <response code="400">400 Erro</response>
    [ProducesResponseType<string>(StatusCodes.Status201Created)]
    [HttpGet("{preferenceId}")]
    [Authorize(Policy = "Auth")]
    public async Task<IActionResult> GetPaymentPreference(string preferenceId)
    {
        var result = await _mediator.Send(new GetPreferenceByIdQuery
        {
            PreferenceId = preferenceId
        });
        
        return Ok(result);
    }
}