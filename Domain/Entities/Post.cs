using Domain.Enums;

namespace Domain.Entities;

public class Post : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int TimesClicked { get; set; }
    public int TimesInteracted { get; set; }
    public int NoOfRooms { get; set; }
    public int Space { get; set; }
    public PropertyType PropertyType { get; set; }
    public bool IsForSelling { get; set; }
    public bool IsForRenting { get; set; }
    public PropertyStatus Status { get; set; }

    public Guid UserId { get; set; }

    public string Thumbnail { get; set; }
    
}