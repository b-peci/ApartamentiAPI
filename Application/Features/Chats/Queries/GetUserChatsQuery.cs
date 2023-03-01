using Application.GlobalDtos;
using Application.Interfaces.Repositories;
using Application.Models;
using MediatR;

namespace Application.Features.Chats.Queries;

public record GetUserChatsQuery(Guid UserId) : IRequest<GlobalResponseDto<List<UserChat>>>; 

public record GetUserChatsQueryHandler(IChatRepository _repository) : IRequestHandler<GetUserChatsQuery, GlobalResponseDto<List<UserChat>>>
{
    public async Task<GlobalResponseDto<List<UserChat>>> Handle(GetUserChatsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userChats = await _repository.GetUserChats(request.UserId);
            var responseDto =
                new GlobalResponseDto<List<UserChat>>(message: "Fetched successfully", isSuccess: true, userChats);
            return responseDto;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}