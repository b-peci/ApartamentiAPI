using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Posts.Commands;

public record DeletePostByIdCommand(Guid PostId) : IRequest<bool>;

public record DeletePostByIdCommandHandler(IPostRepository _postRepository, IImageRepository _imageRepository) : IRequestHandler<DeletePostByIdCommand, bool>
{
    public async Task<bool> Handle(DeletePostByIdCommand request, CancellationToken cancellationToken)
    {
        bool result = await _postRepository.DeletePost(request.PostId);
        if (!result) return false;
        var imageFilePaths = await _imageRepository.GetPostImages(request.PostId);
        foreach (var imageFilePath in imageFilePaths)
        {
            File.Delete(imageFilePath);
        }
        return true;
    }
}