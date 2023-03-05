namespace Application.Interfaces.Repositories;

public interface ICityRepository
{
    Task<IEnumerable<string>> GetCitiesByCountry(string country);
}