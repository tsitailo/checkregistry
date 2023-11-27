namespace CheckRegistry.Domain.Interfaces;

public interface ICheckRegistryRepository
{
    Task<string> AddOrUpdate(Domain.Entities.Check check);
}