namespace Application.Interfaces.Repositories;

public interface IRoleRepository
{
    Task<Guid> GetDefaultRole();
}