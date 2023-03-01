namespace Application.Interfaces.Repositories;

public interface IImageRepository
{
    Task<List<string>> GetPostImages(Guid postId);
}