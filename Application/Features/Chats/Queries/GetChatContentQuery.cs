using Application.GlobalDtos;
using Application.Interfaces.Repositories;
using Domain.Views;
using MediatR;

namespace Application.Features.Chats.Queries;

public record GetChatContentQuery(Guid FirstUserId, Guid SecondUserId) : IRequest<GlobalResponseDto<List<VChatContent>>>;

public record GetChatContentQueryHandler(IChatRepository _repository) : IRequestHandler<GetChatContentQuery, GlobalResponseDto<List<VChatContent>>>
{
    public async Task<GlobalResponseDto<List<VChatContent>>> Handle(GetChatContentQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var chatContent = await _repository.GetChatContent(request.FirstUserId, request.SecondUserId);
            var response = new GlobalResponseDto<List<VChatContent>>("Fetched successfully", isSuccess: true, chatContent);
            /*
             * firstUserId -> is the login user
             * so all the messages that were sent from the secondUserId to firstUserId
             * their status will change to read
             */
            await _repository.SetChatMessagesAsRead(request.SecondUserId, request.FirstUserId);
            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new GlobalResponseDto<List<VChatContent>>("Could not get data", isSuccess: false, new List<VChatContent>());
        }
    }
}