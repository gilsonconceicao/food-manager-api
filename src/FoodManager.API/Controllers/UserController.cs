using AutoMapper;
using FoodManager.API.Services;
using FoodManager.Application.Users.Commands;
using FoodManager.Application.Users.Dtos;
using FoodManager.Application.Users.Queries;
using FoodManager.Domain.Extensions;
using FoodManager.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodManager.API.Controllers;

public class UserController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public UserController(
        IMediator mediator,
        IMapper mapper,
        ITokenService tokenService
    )
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _tokenService = tokenService ?? throw new ArgumentException(nameof(tokenService));
    }

    /// <summary>
    /// Método utilizado para obter usuários
    /// </summary>
    /// <returns>Usuários</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<PagedList<GetUserDto>>(StatusCodes.Status200OK)]
    [HttpGet]
    [Authorize(Policy = "Auth")]
    public async Task<IActionResult> GetList([FromQuery] UserPaginationListQuery query)
    {
        var result = await _mediator.Send(query);

        var foodList = _mapper.Map<List<GetUserDto>>(result.Data);

        var listMappedFromPagination = new PagedList<GetUserDto>(
            data: foodList,
            count: result.Count ?? 0,
            pageNumber: query.Page,
            pageSize: query.Size
        );

        return Ok(listMappedFromPagination);
    }

    /// <summary>
    /// Método utilizado para obter usuário por identificador
    /// </summary>
    /// <returns>Usuários</returns>
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<User>(StatusCodes.Status200OK)]
    [HttpGet("{Id}")]
    [Authorize(Policy = "Auth")]
    public async Task<IActionResult> GetUserByIdAsync(Guid Id)
    {
        var result = await _mediator.Send(new UserGetByIdQuery
        {
            Id = Id
        });
        return Ok(_mapper.Map<GetUserDto>(result));
    }

    /// <summary>
    /// Método utilizado para obter usuário por CPF
    /// </summary>
    /// <returns>Usuários</returns>
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<User>(StatusCodes.Status200OK)]
    [HttpGet("DocumentNumber/{RegistrationNumber}")]
    [Authorize(Policy = "Auth")]
    public async Task<IActionResult> GetUserByRegistratioNumber([FromRoute] string RegistrationNumber)
    {
        var result = await _mediator.Send(new UserGetByRegistrationNumberQuery
        {
            RegistrationNumber = RegistrationNumber
        });
        return Ok(_mapper.Map<GetUserDto>(result));
    }

    /// <summary>
    /// Método utilizado para criar usuários
    /// </summary>
    /// <returns>Usuários</returns>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<Guid>(StatusCodes.Status201Created)]
    [HttpPost]
    [Authorize(Policy = "Auth")]
    public async Task<IActionResult> CreateAsync([FromBody] UserCreateCommand query)
    {
        var decodedToken = await _tokenService.VerifyTokenFromHeaderAsync(Request);

        var result = await _mediator.Send(new UserCreateCommand
        {
            Address = query.Address,
            Email = query.Email,
            FirebaseUserId = decodedToken.UserId,
            Name = query.Name,
            RegistrationNumber = query.RegistrationNumber
        });
        return Ok(result);
    }

    /// <summary>
    /// Método utilizado para atualizar um usuário
    /// </summary>
    /// <returns>Usuários</returns>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<Guid>(StatusCodes.Status201Created)]
    [HttpPut("{Id}")]
    [Authorize(Policy = "Auth")]
    public async Task<IActionResult> CreateAsync([FromRoute] Guid Id, [FromBody] UserUpdateDto model)
    {
        var result = await _mediator.Send(new UserUpdateCommand
        {
            Id = Id,
            Address = model.Address,
            Name = model.Name
        });
        return Ok(result);
    }
}