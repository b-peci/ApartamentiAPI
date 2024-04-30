using Application.Features.Posts.Helper_Methods;
using Application.GlobalDtos;
using Application.Interfaces.Helpers;
using MediatR;
using Serilog;
using SkiaSharp;

namespace Application.Features.Posts.Commands;

public record SavePostImagesCommand(FileDto[] Files, string ThumbnailPath) : IRequest<string[]>;

public record SavePostImagesCommandHandler : IRequestHandler<SavePostImagesCommand, string[]>
{
    private readonly IImageHelper _imageHelper;
    private readonly ILogger _logger;

    public SavePostImagesCommandHandler(IImageHelper imageHelper)
    {
        _imageHelper = imageHelper;
        //_logger = logger;
    }
    

    public async Task<string[]> Handle(SavePostImagesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            string[] filePaths = FileHelper.SaveImagesInFileAndGetTheirPaths(request.Files, "Posts");
            await using var stream = File.OpenRead(filePaths.First());
            await _imageHelper.CreateThumbnailFromImage(stream, request.ThumbnailPath,
                new SKSize(400, 400),
                SKFilterQuality.High, 70);
            return filePaths;
        }
        catch (Exception e)
        {
            _logger.Error(e.Message);
            return [];
        }
        
    }
} 

