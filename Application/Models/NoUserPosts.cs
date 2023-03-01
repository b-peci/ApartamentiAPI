namespace Application.Models;

public class NoUserPosts
{
    public Guid UserId { get; set; }
    public int ImageCountForMonth { get; set; }
    public DateTime PostDate { get; set; }
}