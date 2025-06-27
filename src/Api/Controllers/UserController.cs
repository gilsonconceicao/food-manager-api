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

namespace Api.Controllers;

public class UserController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly SmtpServicesSettings _smtpServicesSetting;
    public UserController(
        IMediator mediator,
        IMapper mapper,
        IOptions<SmtpServicesSettings> smtpSettins
    )
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _smtpServicesSetting = smtpSettins.Value;

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
    [HttpGet("{UserId}")]
    [Authorize(Policy = "Auth")]
    public async Task<IActionResult> GetUserByIdAsync(string UserId)
    {
        var result = await _mediator.Send(new UserGetByIdQuery
        {
            Id = UserId
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
    /// Método utilizado para atualizar um usuário
    /// </summary>
    /// <returns>Usuários</returns>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<Guid>(StatusCodes.Status201Created)]
    [HttpPut("{UserId}")]
    [Authorize(Policy = "Auth")]
    public async Task<IActionResult> UpdateAsync([FromRoute] string UserId, [FromBody] UserUpdateDto model)
    {
        var result = await _mediator.Send(new UserUpdateCommand
        {
            UserId = UserId,
            Address = model.Address,
            PhoneNumber = model.PhoneNumber,
            Name = model.Name
        });
        return Ok(result);
    }
    
    /// <summary>
    /// Método que sincroniza usuários
    /// </summary>
    [HttpPost("Sync")]
    public async Task<IActionResult> SyncUsers()
    {
        var result = await _mediator.Send(new MergeUsersFirebaseCommand{});
        return Ok(result);
    }
    
    /// <summary>
    /// Método utilizado para slvar um usuário
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateUserAsync([FromBody] UserCreateCommand model)
    {
        var result = await _mediator.Send(model);
        return Ok(result);
    }
}