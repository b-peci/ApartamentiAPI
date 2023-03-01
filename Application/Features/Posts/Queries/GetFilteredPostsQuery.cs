using Application.Features.Posts.ViewModels;
using Application.Interfaces.Repositories;
using Domain.Enums;
using MediatR;

namespace Application.Features.Posts.Queries;

public record GetFilteredPostsQuery(string Country, string City, int Status, int Type, decimal MinPrice,
    decimal MaxPrice, int NoRooms, int PageNumber) : IRequest<PostThumbnailViewModel>;

public record GetFilteredPostsQueryHandler
    (IReadPostRepository _repository) : IRequestHandler<GetFilteredPostsQuery, PostThumbnailViewModel>
{
    public async Task<PostThumbnailViewModel> Handle(GetFilteredPostsQuery request, CancellationToken cancellationToken)
    {
        var model = new PostThumbnailViewModel();
        var filteredPosts = await _repository.GetFilteredPostsThumbnail(request.Country, request.City, (PropertyStatus)request.Status,
            (PropertyType)request.Type, request.MinPrice, request.MaxPrice, request.NoRooms, request.PageNumber);
        model.Posts = filteredPosts;
        model.PageNumber = request.PageNumber;
        model.TotalItems = model.Posts.Count();
        return model;
    }
}
    