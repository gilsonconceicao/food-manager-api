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

namespace Api.Controllers;

public class UserController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ICurrentUser _httpUserService;
    private readonly IMapper _mapper;

    public UserController(
        IMediator mediator,
        IMapper mapper,
        ICurrentUser httpUserService
    )
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _httpUserService = httpUserService ?? throw new ArgumentException(nameof(httpUserService));
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
    /// Método utilizado para simcronizar os usuários
    /// </summary>
    /// <returns>Usuários</returns>
    [HttpPost("MergeUsers")]
    [Authorize(Policy = "Auth")]
    public async Task<IActionResult> MergeUsersAsync()
    {
        await _mediator.Send(new MergeUsersFirebaseCommand{});
        return NoContent();
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
    /// Método utilizado para validar se o usuário é master
    /// </summary>
    /// <returns>Usuários</returns>
    [HttpGet("VerifyUserIsMaster/{FirebaseUserId}")]
    [Authorize(Policy = "Auth")]
    public async Task<IActionResult> VerifyUserIsMasterAsync(string FirebaseUserId)
    {
        var result = await _mediator.Send(new VerifyUserIsMasterQuery
        {
            FirebaseUserId = FirebaseUserId
        });
        return Ok(result);
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
        var decodedToken = await _httpUserService.GetAuthenticatedUser();

        var result = await _mediator.Send(new UserCreateCommand
        {
            Address = query.Address,
            Email = query.Email,
            FirebaseUserId = decodedToken.UserId,
            Name = query.Name,
            PhoneNumber = query.PhoneNumber
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