using System.Drawing;
using SkiaSharp;

namespace Application.Interfaces.Helpers;

public interface IImageHelper
{
    Task CreateThumbnailFromImage(Stream fullSizedImage, string targetPath, SKSize thumbnailSize, SKFilterQuality filterQuality, int encodeQuality);
}