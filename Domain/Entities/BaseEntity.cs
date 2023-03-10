namespace Domain.Entities;

public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public Guid? LastUpdatedBy { get; set; }
    public int LastUpdatedNo { get; set; }
}