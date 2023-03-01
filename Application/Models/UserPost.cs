using Domain.Enums;

namespace Application.Models;

public class UserPost
{
    public Guid UserId { get; set; }
    public Guid Id { get; set; }
    public string Title { get; set; }
    public double Space { get; set; }
    public PropertyType Type { get; set; }
}