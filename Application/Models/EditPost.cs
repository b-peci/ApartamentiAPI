using Application.GlobalDtos;
using Domain.Enums;

namespace Application.Models;

public class EditPost : IDto
{
    public EditPost()
    {
        Files = new();
    }
    public Guid PostId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsForRenting { get; set; }
    public bool IsForSelling { get; set; }
    public decimal Price { get; set; }
    public int NoRooms { get; set; }
    public PropertyType PropertyType { get; set; }
    public int Space { get; set; }
    private string[] Images
    {
        set
        {
            foreach (var item in value)
            {
                Files.Add(new FileDto()
                {
                    Base64Content = Convert.ToBase64String(File.ReadAllBytes(item)),
                    Extension = item.Split('.')[1]
                });
            }
        }
    }
    public List<FileDto> Files { get; set; }
    public PropertyStatus Status { get; set; }
}