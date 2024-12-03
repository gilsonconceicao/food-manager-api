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

namespace Api.Controllers;

public class PaymentController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IHttpUserService _httpUserService;
    private readonly IMapper _mapper;
    private readonly IPixCommunication _pixCommunication; 

    public PaymentController(
        IMediator mediator,
        IMapper mapper,
        IHttpUserService tokenService,
        IPixCommunication pixCommunication
    )
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _httpUserService = tokenService ?? throw new ArgumentException(nameof(tokenService));
                _pixCommunication = pixCommunication ?? throw new ArgumentException(nameof(pixCommunication));
    }

    [HttpGet]
    public IActionResult GetAlgo() 
    {
        
        return Ok(_pixCommunication.CreatePixAsync());
    }

}