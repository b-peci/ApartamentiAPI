using Application.Interfaces.Helpers;
using System.Drawing;
using Serilog;
using SkiaSharp;

namespace Infrastructure.Implementations.Helpers;

public class ImageHelper : IImageHelper
{
    public async Task CreateThumbnailFromImage(Stream fullSizedImage, string targetPath, SKSize thumbnailSize, SKFilterQuality filterQuality, int encodeQuality)
    {
        await Task.Run(() =>
        {
            using var bitmap = SKBitmap.Decode(fullSizedImage);
            float scale = Math.Min(thumbnailSize.Width / bitmap.Width, thumbnailSize.Height / bitmap.Height);

            float newWidth = (bitmap.Width * scale);
            float newHeight = (bitmap.Height * scale);

            using var resizedImage = bitmap.Resize(new SKImageInfo((int)newWidth, (int)newHeight), filterQuality);
            if (resizedImage == null) throw new Exception("Image could not be resized");
            using var output = File.OpenWrite(targetPath);
            using var image = SKImage.FromBitmap(resizedImage);
            using var data = image.Encode(SKEncodedImageFormat.Jpeg, encodeQuality);
            data.SaveTo(output);
        });
    }
}