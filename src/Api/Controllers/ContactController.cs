
using Application.Contacts.Commands;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ContactController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ContactController(
        IMediator mediator,
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Envia uma mensagem de contato
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> AddCartAsync([FromBody] ContactCreateCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

}