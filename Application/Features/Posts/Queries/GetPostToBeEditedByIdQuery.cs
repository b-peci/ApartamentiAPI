using Application.GlobalDtos;
using Application.Interfaces.Repositories;
using Application.Models;
using MediatR;
using Serilog;

namespace Application.Features.Posts.Queries;

public record GetPostToBeEditedByIdQuery(Guid PostId) : IRequest<GlobalResponseDto<EditPost>>;


public record GetPostToBeEditedByIdQueryHandler(IPostRepository _repository) : IRequestHandler<GetPostToBeEditedByIdQuery, GlobalResponseDto<EditPost>>
{
    public async Task<GlobalResponseDto<EditPost>> Handle(GetPostToBeEditedByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // IDEA: Just add user id here, and check whether there is any post with that user id and postid
            EditPost postToBeEdited = await _repository.GetPostToBeEdited(request.PostId);
            
            return new GlobalResponseDto<EditPost>("Fetched data", isSuccess: true, postToBeEdited);
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            return new GlobalResponseDto<EditPost>("Could not get data", isSuccess: false);
        }
    }
}