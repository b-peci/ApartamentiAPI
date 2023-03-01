namespace Domain.Entities;

public class Image : BaseEntity
{
    public string FilePath { get; set; }
    public bool IsMainImage { get; set; }

    public Guid PostId { get; set; }
}