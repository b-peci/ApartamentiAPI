using Application.GlobalDtos;
using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Cities.Queries;

public record GetCitiesByCountryQuery(string Country)  : IRequest<GlobalResponseDto<IEnumerable<string>>>;


public record GetCitiesByCountryQueryHandler(ICityRepository _repository) : IRequestHandler<GetCitiesByCountryQuery, GlobalResponseDto<IEnumerable<string>>>
{
    public async Task<GlobalResponseDto<IEnumerable<string>>> Handle(GetCitiesByCountryQuery request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetCitiesByCountry(request.Country);
        var response = new GlobalResponseDto<IEnumerable<string>>(message: "Fetched cities successfully",
            isSuccess: true, data);
        return response;
    }
}