using Application.Features.Posts.Commands;
using Application.Features.Posts.Queries;
using Host.HelperMethods;
using MediatR;
using Serilog;

namespace Host.EndPointMethods;

public class PostEndPoints
{
    public static async Task<IResult> GetPostsThumbnail(IMediator mediator,int pageNumber)
    {
        try
        {
            if (pageNumber <= 0)
                pageNumber = 1;
            return Results.Ok(await mediator.Send(new GetPostsThumbnailQuery(pageNumber)));
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }
    
    public static async Task<IResult> GetFilteredPosts(IMediator _mediator, string country, string city, int status, int type,
        decimal minPrice, decimal maxPrice, int noRooms, int pageNumber)
    {
        try
        {
            return Results.Ok(await _mediator.Send(new GetFilteredPostsQuery(country, city, status, type, minPrice,
                maxPrice, noRooms, pageNumber)));
        }
        catch (Exception e)
        {
            return Results.BadRequest("Could not filter posts");
        }
    }


    public static async Task<IResult> AddPost(IMediator mediator, AddPostCommand command)
    {
        try
        {
            command.UserId = ClaimFunctionality.GetUserIdFromClaim();
            return await mediator.Send(command);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.Problem(e.Message);
        }
    }

    public static async Task<IResult> GetUserPostCount(IMediator mediator)
    {
        try
        {
            Guid userId = ClaimFunctionality.GetUserIdFromClaim();
            return Results.Ok(await mediator.Send(new GetNoOfUserPostsQuery(userId)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.Problem(e.Message);
        }
    }

    public static async Task<IResult> GetPostsInteractedPosts(IMediator mediator)
    {
        try
        {
            Guid userId = ClaimFunctionality.GetUserIdFromClaim();
            return Results.Ok(await mediator.Send(new GetPostsInteractedTimeQuery(userId)));
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            return Results.Problem(e.Message);
        }
    }

    public static async Task<IResult> GetUserPosts(IMediator mediator, int pageNumber)
    {
        Guid userId = ClaimFunctionality.GetUserIdFromClaim();
        return Results.Ok(await mediator.Send(new GetUserPostsQuery(userId, pageNumber)));
    }
    
    // TODO make so only the one who created can access the data in here

    public static async Task<IResult> GetPostToEditById(IMediator mediator, Guid postId)
    {
        try
        {
            return  Results.Ok(await mediator.Send(new GetPostToBeEditedByIdQuery(postId)));
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            return Results.BadRequest(e.Message);
        }
    }
    
    
    
    public static async Task<IResult> GetPostById(IMediator mediator, Guid postId)
    {
        try
        {
            return  Results.Ok(await mediator.Send(new GetPostToBeEditedByIdQuery(postId)));
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            return Results.BadRequest(e.Message);
        }
    }

    public static async Task<IResult> DeletePost(IMediator mediator, Guid postId)
    {
        try
        {
            return Results.Ok(await mediator.Send(new DeletePostByIdCommand(postId)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    // TODO make so only the one who created can access this endpoint
    public static async Task<IResult> UpdatePost(IMediator mediator, UpdatePostDataCommand command)
    {
        try
        {
            return Results.Ok(await mediator.Send(command));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static async Task<IResult> IsUserCreator(IMediator _mediator, Guid postId)
    {
        try
        {
            Guid userId = ClaimFunctionality.GetUserIdFromClaim();
            return Results.Ok(await _mediator.Send( new IsPostCreatorQuery(userId, postId)));
        }
        catch (Exception e)
        {
            return Results.BadRequest("Could not check");
        }
    }


  
}