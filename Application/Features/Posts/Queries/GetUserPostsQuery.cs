using Application.Interfaces.Repositories;
using Application.Models;
using MediatR;

namespace Application.Features.Posts.Queries;

public record GetUserPostsQuery(Guid UserId, int PageNumber) : IRequest<List<UserPost>>;

public record GetUserPostsQueryHandler(IPostRepository _repository) : IRequestHandler<GetUserPostsQuery, List<UserPost>>
{
    public async Task<List<UserPost>> Handle(GetUserPostsQuery request, CancellationToken cancellationToken)
    {
        var response = await _repository.GetUserPosts(request.UserId, request.PageNumber);
        return response;
    }
}