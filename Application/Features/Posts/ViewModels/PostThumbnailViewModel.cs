using Application.Models;

namespace Application.Features.Posts.ViewModels;

public class PostThumbnailViewModel
{
    public IEnumerable<PostThumbnailData> Posts { get; set; }
    public int PageNumber { get; set; }
    public int TotalItems { get; set; }
}