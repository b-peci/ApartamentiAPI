using Application.Features.Posts.Helper_Methods;
using Application.GlobalDtos;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Posts.Commands;

public record UpdatePostDataCommand(Guid PostId, string Title, string Description, PropertyStatus Status, int NoRooms, int Space, 
    decimal Price, PropertyType Type, bool IsForSelling, bool IsForRenting, FileDto[] Base64EncodedImages) : IRequest<bool>;

public record UpdatePostDataCommandHandler(IPostRepository _repository) : IRequestHandler<UpdatePostDataCommand, bool>
{
    public async Task<bool> Handle(UpdatePostDataCommand request, CancellationToken cancellationToken)
    {
        string[] filePaths = FileHelper.SaveImagesInFileAndGetTheirPaths(request.Base64EncodedImages, "Posts");
        var post = new Post()
        {
            Id = request.PostId,
            Title = request.Title,
            Description = request.Description,
            Status = request.Status,
            NoOfRooms = request.NoRooms,
            Space = request.Space,
            Price = request.Price,
            PropertyType = request.Type,
            IsForSelling = request.IsForSelling,
            IsForRenting = request.IsForRenting
        };
        var result = await _repository.UpdatePost(post, filePaths);
        return result;
    }
}
