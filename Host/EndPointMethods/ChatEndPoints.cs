using Application.Features.Chats.Commands;
using Application.Features.Chats.Queries;
using Application.Features.Posts.Commands;
using Host.HelperMethods;
using MediatR;

namespace Host.EndPointMethods;

public class ChatEndPoints
{
    public static async Task<IResult> GetChats(IMediator mediator)
    {
        Guid userId = ClaimFunctionality.GetUserIdFromClaim();
        return Results.Ok(await mediator.Send(new GetUserChatsQuery(userId)));
    }
    public static async Task<IResult> GetChatContent(IMediator mediator, Guid secondUserId)                     
    {                                                                                  
        Guid firstUserId = ClaimFunctionality.GetUserIdFromClaim();                         
        return Results.Ok(await mediator.Send(new GetChatContentQuery(firstUserId, secondUserId)));         
    }
    
    public static async Task<IResult> SendMessage(IMediator mediator, SentMessageCommand command)                     
    {                                                                                  
        command.FromUserId = ClaimFunctionality.GetUserIdFromClaim();                         
        return Results.Ok(await mediator.Send(command));         
    } 
   
}