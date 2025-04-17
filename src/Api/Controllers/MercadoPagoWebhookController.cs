using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Hangfire;
using Application.Workflows.Workflows;
using MediatR;
using Application.Payment.Commands;

namespace Api.Controllers
{
    [ApiController]
    [Route("webhooks/mercadopago")]
    public class MercadoPagoWebhookController : ControllerBase
    {
        private readonly ILogger<MercadoPagoWebhookController> _logger;
        private readonly IPaymentCommunication _paymentCommunication;
        private readonly IMediator _mediator;


        public MercadoPagoWebhookController(
            ILogger<MercadoPagoWebhookController> logger,
            IPaymentCommunication paymentCommunication,
            IMediator mediator)
        {
            _logger = logger;
            _paymentCommunication = paymentCommunication;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromQuery] string id, [FromQuery] string topic)
        {
            _logger.LogInformation("🔔 Webhook recebido | Topic: {topic} | Id: {id}", topic, id);

            if (string.IsNullOrWhiteSpace(topic) || string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("🚫 Webhook inválido | topic ou ID is null | Topic: {topic} | Id: {id}", topic, id);
                return BadRequest("Topic ou ID ausente.");
            }

            switch (topic)
            {
                case "merchant_order":
                    await _paymentCommunication.ProcessMerchantOrderWebhookAsync(id);
                    break;
                case "payment":
                    await _mediator.Send(new ProcessMerchantOrderWebhookCommand
                    {
                        PaymentId = id
                    });
                    break;
                default:
                    return BadRequest("Topic não suportado.");
            }

            return Ok();
        }
    }
}
