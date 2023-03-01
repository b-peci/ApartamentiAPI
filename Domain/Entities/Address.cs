namespace Domain.Entities;

public class Address : BaseEntity
{
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public double Latitude { get; set; }
    public double Longtitude { get; set; }
    
    public Guid PostId { get; set; }

    
}