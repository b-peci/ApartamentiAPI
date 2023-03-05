using Application.Features.Chats.ViewModels;
using Application.Models;
using Domain.Views;

namespace Application.Interfaces.Repositories;

public interface IChatRepository
{
    Task<List<UserChat>> GetUserChats(Guid userId);
    Task<List<VChatContent>> GetChatContent(Guid firstUserId, Guid secondUserId);
    Task AddMessage(SentMessageViewModel viewModel);
    Task SetChatMessagesAsRead(Guid fromUserId, Guid toUserId);
}