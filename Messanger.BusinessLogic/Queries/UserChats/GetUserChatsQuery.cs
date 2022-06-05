﻿using MediatR;
using Messanger.BusinessLogic.Models;
using Messanger.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Messanger.BusinessLogic.Queries.Chats;

public class GetUserChatsQuery : BaseRequest, IRequest<Response<IEnumerable<Chat>>>
{

}

public class GetUserChatsHandler : IRequestHandler<GetUserChatsQuery, Response<IEnumerable<Chat>>>
{
    private readonly DatabaseContext _context;
    
    public GetUserChatsHandler(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<Response<IEnumerable<Chat>>> Handle(GetUserChatsQuery request, CancellationToken cancellationToken)
    {
        var userChats = await _context.UserChats
            .AsNoTracking()
            .Include(x => x.Chat)
            .ThenInclude(x => x.Messages)
            .Where(x => x.UserId == request.UserId)
            .Select(x => new Chat
            {
                Id = x.Chat.Id,
                Title = x.Chat.Title,
                MembersCount = x.Chat.MembersCount
            }).Take(100).ToListAsync(cancellationToken);

        return Response.Ok<IEnumerable<Chat>>("Ok", userChats);

    }
}