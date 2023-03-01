namespace Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; }
    public string Password { get; set; }
    public byte[] Salt { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public bool IsFromOAUTH { get; set; }

    public User(string email, string password, byte[] salt, string name, string lastName, DateTime? dateOfBirth, Guid roleId, bool isFromOauth)
    {
        Email = email;
        Password = password;
        Salt = salt;
        Name = name;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        RoleId = roleId;
        IsFromOAUTH = isFromOauth;
    }
    
    public Guid RoleId { get; set; }
}