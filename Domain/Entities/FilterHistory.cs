namespace Domain.Entities;

public class FilterHistory : BaseEntity
{
    public string FilterBy { get; set; }
    public string FilterValue { get; set; }

    public Guid UserId { get; set; }
}