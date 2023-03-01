namespace Application.Models;

public class UserChat
{
    public Guid SentFromUserId { get; set; }
    public string FullName { get; set; }
    public DateTime LastDateMessage { get; set; }
    public string LastMassage { get; set; }
    public int UnReadMessageCount { get; set; }
}