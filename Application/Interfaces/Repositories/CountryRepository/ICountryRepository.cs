namespace Application.Interfaces.Repositories;

public interface ICountryRepository
{
    Task<IEnumerable<string>> GetCountries();
}