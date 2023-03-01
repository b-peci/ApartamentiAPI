using Application.Features.Chats.ViewModels;
using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Chats.Commands;

public class SentMessageCommand : IRequest<bool>
{
    public Guid FromUserId { get; set; }
    public Guid ToUserId { get; set; }
    public string Message { get; set; }
} 

public record SentMessageCommandHandler(IChatRepository _repository) : IRequestHandler<SentMessageCommand, bool>
{
    public async Task<bool> Handle(SentMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var viewModel = new SentMessageViewModel(request.FromUserId, request.ToUserId, request.Message);
            await _repository.AddMessage(viewModel);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}
