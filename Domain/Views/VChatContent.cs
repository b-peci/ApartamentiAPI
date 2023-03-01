namespace Domain.Views;

public class VChatContent
{
    public string SenderFullName { get; set; }
    public string Message { get; set; }
    public DateTime TimeSent { get; set; }
    public bool HasBeenRead { get; set; }
    public Guid FromUserId { get; set; }
}