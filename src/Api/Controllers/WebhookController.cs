using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;

namespace Api.Controllers
{
    [ApiController]
    [Route("webhooks/mercadopago")]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger<WebhookController> _logger;
        private readonly IPaymentCommunication _paymentCommunication;

        public WebhookController(
            ILogger<WebhookController> logger,
            IPaymentCommunication paymentCommunication)
        {
            _logger = logger;
            _paymentCommunication = paymentCommunication;
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveNotification()
        {
            try
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();

                var query = Request.Query;
                var topic = query["topic"].ToString();
                var id = query["id"].ToString();

                _logger.LogInformation("Webhook recebido. Topic: {topic}, ID: {id}", topic, id);

                if (topic == "payment" && !string.IsNullOrEmpty(id))
                {

                    await _paymentCommunication.ProcessPaymentWebhookAsync(id);
                }
                else
                {
                    _logger.LogWarning("Webhook de tipo não reconhecido: {topic}", topic);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar webhook do Mercado Pago");
                return BadRequest();
            }
        }
    }
}
