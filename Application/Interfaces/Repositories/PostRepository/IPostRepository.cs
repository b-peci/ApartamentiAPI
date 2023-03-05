using Application.Models;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IPostRepository
{
    Task<IEnumerable<PostThumbnailData>> GetPostsThumbnail(int pageNumber);
    Task<int> GetPostsTotalCount();
    Task AddPost(Post post, string[] imagesPath);
    Task<List<NoUserPosts>> GetPostsCountForMonth(Guid userId);
    Task AddPostTimeInteracted(List<Guid> postIds, Guid userId);
    Task<List<NoUserPosts>> GetTimesInteracted(List<Guid> postIds);
    Task<List<Guid>> GetUserPostIds(Guid userId);
    Task<List<UserPost>> GetUserPosts(Guid userId, int pageNumber);
    Task<EditPost> GetPostToBeEdited(Guid postId);
    Task<bool> DeletePost(Guid postId);
    Task<bool> UpdatePost(Post post, string[] imagePaths);
}