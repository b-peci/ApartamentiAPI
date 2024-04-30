namespace Application.Models;

public class PostThumbnailData
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    private string _thumbnail;

    public string Thumbnail
    {
        get
        {
            return _thumbnail;
        }
        set
        {
            _thumbnail = value;
            Base64MainImage = Convert.ToBase64String(File.ReadAllBytes(value));
            ImageExtension = value.Split('.')[1];
        }
    }

    public decimal Price { get; set; }
    public string? Base64MainImage { get; set; }
    public string? ImageExtension { get; set; }
}