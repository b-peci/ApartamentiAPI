using Application.Features.Posts.ViewModels;
using Application.Interfaces.Repositories;
using MediatR;
using Serilog;

namespace Application.Features.Posts.Queries;

public record GetPostsInteractedTimeQuery(Guid UserId) : IRequest<UserCountPostsForMonth>;

public record GetPostsInteractedTimeQueryHandler(IPostRepository _repository) : IRequestHandler<GetPostsInteractedTimeQuery, UserCountPostsForMonth>
{
    public async Task<UserCountPostsForMonth> Handle(GetPostsInteractedTimeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var model = new UserCountPostsForMonth();
            List<Guid> postIds = await _repository.GetUserPostIds(request.UserId);
            if (postIds.Any())
                model.NoPosts = await _repository.GetTimesInteracted(postIds);
            return model;
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            return new UserCountPostsForMonth
            {
                ErrorMessage = "Could not get posts interacted"
            };
        }
        
    }
} 