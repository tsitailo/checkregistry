namespace CheckRegistry.Domain.Interfaces;

public interface ICheckRegistryRepository
{
    Task<string> AddOrUpdate(Domain.Entities.Check check);

    //Task<Operation> GetById(string id);
    //Task<Operation> GetByCRunnerId(string crunnerId);
    //Task<Operation> GetByNWorkerIdAndOperationType(string nworkerId, OperationType operationType);
    //Task<List<Operation>> GetUnfinishedOperations();
}