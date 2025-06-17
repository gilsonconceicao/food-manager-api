using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Payment.Commands;

namespace Api.Controllers
{
    [ApiController]
    [Route("webhooks/mercadopago")]
    public class MercadoPagoWebhookController : ControllerBase
    {
        private readonly ILogger<MercadoPagoWebhookController> _logger;
        private readonly IMediator _mediator;


        public MercadoPagoWebhookController(
            ILogger<MercadoPagoWebhookController> logger,
            IMediator mediator)
        {
            _logger = logger;
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
