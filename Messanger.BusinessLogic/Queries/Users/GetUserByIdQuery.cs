﻿using MediatR;
using Messanger.BusinessLogic.Models;
using Messanger.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Messanger.BusinessLogic.Queries.Users;

public class GetUserByIdQuery : IRequest<Response<User>>
{
    public Guid UserId { get; set; }
}

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery,Response<User>>
{
    private readonly DatabaseContext _context;

    public GetUserByIdHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Response<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context
            .Users
            .AsNoTracking()
            .Where(userEntity => userEntity.Id == request.UserId)
            .Select(userEntity => new User
            {
                Email = userEntity.Email,
                Password = userEntity.Password,
                Username = userEntity.Username
            }).SingleOrDefaultAsync(user => user.UserId == request.UserId);

        if (user == null)
        {
            Response.Fail<User>("User not found");
        }
        
        return Response.Ok("Ok", user);
    }
}