using Application.Features.Countries.Query;
using Application.Features.Posts.Commands;
using Host.HelperMethods;
using MediatR;

namespace Host.EndPointMethods;

public class CountryEndpoints
{
    public static async Task<IResult> GetCountries(IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(new GetCountriesQuery()));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.Problem("Could not get countries");
        }
    }
}