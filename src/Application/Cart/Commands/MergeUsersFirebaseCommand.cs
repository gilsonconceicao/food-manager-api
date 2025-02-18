
using Api.Services;
using AutoMapper;
using Domain.Models;
using FirebaseAdmin.Auth;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Carts.Commands;

public class MergeUsersFirebaseCommand : IRequest<bool>
{
}

public class MergeUsersFirebaseCommandHandler : IRequestHandler<MergeUsersFirebaseCommand, bool>
{
    private readonly DataBaseContext _context;
    private readonly ICurrentUser _httpUserService;
    private readonly IMapper _mapper;

    public MergeUsersFirebaseCommandHandler(
        DataBaseContext context,
        ICurrentUser httpUserService,
        IMapper mapper
    )
    {
        _context = context;
        _httpUserService = httpUserService;
        _mapper = mapper;
    }

    public async Task<bool> Handle(MergeUsersFirebaseCommand request, CancellationToken cancellationToken)
    {
        var exportedUsers = await _httpUserService.GetExportedUserRecords();
        var currentUsers = await _context.Users.ToListAsync();
        var usersMapped = new List<User>();

        foreach (var user in exportedUsers)
        {
            var userExists = currentUsers.FirstOrDefault(x => x.FirebaseUserId == user.Uid);
            if (userExists == null)
            {
                usersMapped.Add(
                    _mapper.Map<ExportedUserRecord, User>(user)
                );
            }
        }

        await _context.Users.AddRangeAsync(usersMapped);
        await _context.SaveChangesAsync();
        return true;
    }
}