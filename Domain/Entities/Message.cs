namespace Domain.Entities;

public class Messages : BaseEntity
{
    public string Message { get; set; }
    public bool HasBeenRead { get; set; }
    public Guid FromUserId { get; set; }
    public Guid ToUserId { get; set; }
}