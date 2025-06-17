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
    [HttpPost]
    [Authorize(Policy = "Auth")]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentCommand createPaymentCommand)
    {
        var result = await _mediator.Send(createPaymentCommand);
        return Ok(result);
    }

    /// <summary>
    /// Realiza uma consulta de pagamento diretamente na API do Mercado Pago
    /// </summary>
    /// <response code="200">200 Sucesso</response>
    /// <response code="400">400 Erro</response>
    [ProducesResponseType<string>(StatusCodes.Status201Created)]
    [HttpGet("{paymentId}")]
    [Authorize(Policy = "Auth")]
    public async Task<IActionResult> GetPaymentPreference(long paymentId)
    {
        var result = await _mediator.Send(new GetPaymentByIdQuery
        {
            PaymentId = paymentId
        });
        
        return Ok(result);
    }
}