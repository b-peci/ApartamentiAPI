using Application.Features.Posts.ViewModels;
using Application.Interfaces.Repositories;
using MediatR;
using Serilog;

namespace Application.Features.Posts.Queries;

public record GetNoOfUserPostsQuery(Guid UserId) : IRequest<UserCountPostsForMonth>;

public record GetNoOfUserPostsQueryHandler(IPostRepository _repository) : IRequestHandler<GetNoOfUserPostsQuery, UserCountPostsForMonth>
{
    public async Task<UserCountPostsForMonth> Handle(GetNoOfUserPostsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = new UserCountPostsForMonth();
            response.NoPosts = 
                await _repository.GetPostsCountForMonth(request.UserId);
            return response;
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            throw;
        }
    }
} 