using Application.Features.Posts.ViewModels;
using Application.Interfaces.Repositories;
using MediatR;
using Serilog;

namespace Application.Features.Posts.Queries;

public class GetPostsThumbnailQuery : IRequest<PostThumbnailViewModel>
{
    public int PageNumber { get; set; }

    public GetPostsThumbnailQuery(int pageNumber)
    {
        PageNumber = pageNumber;
    }
}

public class GetPostsThumbnailQueryHandler : IRequestHandler<GetPostsThumbnailQuery,PostThumbnailViewModel>
{
    private readonly IPostRepository _repository;
    public GetPostsThumbnailQueryHandler(IPostRepository repository)
    {
        _repository = repository;
        
    }

    public async Task<PostThumbnailViewModel> Handle(GetPostsThumbnailQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var model = new PostThumbnailViewModel();
            model.Posts = await _repository.GetPostsThumbnail(request.PageNumber);
            model.TotalItems = await _repository.GetPostsTotalCount();
            model.PageNumber = request.PageNumber;
            
            return model;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Log.Error($"The error is {e}", e);
            throw e;
        }
    }
} 