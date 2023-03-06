using Application.GlobalDtos;
using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Countries.Query;

public record GetCountriesQuery() : IRequest<GlobalResponseDto<IEnumerable<string>>>;


public record GetCountriesQueryHandler(ICountryRepository _repository) 
    : IRequestHandler<GetCountriesQuery, GlobalResponseDto<IEnumerable<string>>>
{
    public async Task<GlobalResponseDto<IEnumerable<string>>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetCountries();

        var response = new GlobalResponseDto<IEnumerable<string>>("Countries fetched", isSuccess: true,
            data: data);
        return response;
    }
}