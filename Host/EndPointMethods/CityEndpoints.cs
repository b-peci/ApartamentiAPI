using Application.Features.Cities.Queries;
using Application.Features.Countries.Query;
using MediatR;

namespace Host.EndPointMethods;

public class CityEndpoints
{
    public static async Task<IResult> GetCities(IMediator mediator, string country)
    {
        try
        {
            return Results.Ok(await mediator.Send(new GetCitiesByCountryQuery(country)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.Problem("Could not get countries");
        }
    }
}