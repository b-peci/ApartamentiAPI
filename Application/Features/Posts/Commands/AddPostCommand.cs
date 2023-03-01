using Application.Features.Posts.Helper_Methods;
using Application.GlobalDtos;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

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

public class AddPostCommandHandler : IRequestHandler<AddPostCommand, IResult>
{
    private readonly IPostRepository _repository;

    public AddPostCommandHandler(IPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<IResult> Handle(AddPostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            string[] filePaths = FileHelper.SaveImagesInFileAndGetTheirPaths(request.Base64EncodedImages, "Posts");
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
                UserId = request.UserId
            };
            await _repository.AddPost(post, filePaths);
            return Results.Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.Problem(e.Message);
        }
    }


}