namespace Application.Features.Chats.ViewModels;

public record SentMessageViewModel(Guid FromUserId, Guid ToUserId, string Message);