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
using Application.Carts.Commands;
using Integrations.Settings;
using Microsoft.Extensions.Options;
using Domain.Interfaces;

namespace Api.Controllers;

public class MercadoPagoController  : BaseController
{
    private readonly ILogger<MercadoPagoController> _logger;
    private readonly IPaymentCommunication _paymentCommunication;

    public MercadoPagoController(
        ILogger<MercadoPagoController> logger,
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
            // Lê o corpo da requisição enviado pelo Mercado Pago
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

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar webhook do Mercado Pago");
            return BadRequest();
        }
    }
}