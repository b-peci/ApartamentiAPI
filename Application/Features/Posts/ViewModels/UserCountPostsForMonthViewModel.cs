using Application.Models;

namespace Application.Features.Posts.ViewModels;

public class UserCountPostsForMonth
{
    public List<NoUserPosts> NoPosts { get; set; }
    public string ErrorMessage { get; set; }

    public UserCountPostsForMonth()
    {
        NoPosts = new List<NoUserPosts>();
    }
}

