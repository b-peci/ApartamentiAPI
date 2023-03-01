namespace Domain.Entities;

public class PostViewed : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid PostId { get; set; }
}