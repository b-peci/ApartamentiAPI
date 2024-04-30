using Application.Features.Posts.Helper_Methods;
using Application.GlobalDtos;
using Application.Interfaces.Helpers;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using SkiaSharp;

namespace Application.Features.Posts.Commands;

public class AddPostCommand : IRequest<IResult>
{
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public PropertyStatus Status { get; set; }
    public int NoRooms { get; set; }
    public int Space { get; set; }
    public decimal Price { get; set; }
    public PropertyType Type { get; set; }
    public bool IsForSelling { get; set; }
    public bool IsForRenting { get; set; }
    public FileDto[] Base64EncodedImages { get; set; }
}

public class AddPostCommandHandler(IPostRepository repository, IMediator mediator)
    : IRequestHandler<AddPostCommand, IResult>
{
    public async Task<IResult> Handle(AddPostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            string databaseFileDirectoryPath = GlobalFunctions.GetDatabaseDirectoryPath("Posts");
            string thumbnailPath = $"{databaseFileDirectoryPath}/thumbnail_{Guid.NewGuid().ToString()}.{request.Base64EncodedImages.First().Extension}";
            string[] filePaths = await mediator.Send(new SavePostImagesCommand(request.Base64EncodedImages, thumbnailPath), cancellationToken);
            if (filePaths.Length == 0) return Results.Problem("Could not create post");
            
            var post = new Post
            {
                Description = request.Description,
                IsForRenting = request.IsForRenting,
                IsForSelling = request.IsForSelling,
                Title = request.Title,
                Price = request.Price,
                NoOfRooms = request.NoRooms,
                Space = request.Space,
                Status = PropertyStatus.Active,
                PropertyType = request.Type,
                UserId = request.UserId,
                Thumbnail = thumbnailPath
            };
            await repository.AddPost(post, filePaths);
            return Results.Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.Problem("Could not create post");
        }
    }


}