using AutoMapper;
using FoodManager.Application.Users.Dtos;
using FoodManager.Application.Users.Queries;
using FoodManager.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodManager.API.Controllers;

public class UserController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UserController(IMediator mediator,
    IMapper mapper)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Método utilizado para obter usuários
    /// </summary>
    /// <returns>Usuários</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<PagedList<GetUserDto>>(StatusCodes.Status200OK)]
    [HttpGet]
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
}