using Application.Models;
using Domain.Enums;

namespace Application.Interfaces.Repositories;

public interface IReadPostRepository
{
    Task<bool> IsUserPostCreator(Guid userId, Guid postId);
    Task<IEnumerable<PostThumbnailData>> GetFilteredPostsThumbnail(string country, string city, PropertyStatus status, PropertyType type,
        decimal minPrice, decimal maxPrice, int noRooms, int pageNumber);

}