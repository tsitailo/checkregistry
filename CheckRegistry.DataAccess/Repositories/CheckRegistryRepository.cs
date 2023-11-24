using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using CheckRegistry.Domain.Interfaces;

namespace CheckRegistry.DataAccess.Repositories;

public class CheckRegistryRepository : ICheckRegistryRepository
{
    private readonly IDynamoDBContext _dbContext;
    private readonly IMapper _mapper;

    public CheckRegistryRepository(IDynamoDBContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    //public async Task<DomainOperation> GetById(string id)
    //{
    //    var asyncSearch = _dbContext.QueryAsync<DBOperation>(id, QueryOperator.Equal, new[] {id},
    //        new DynamoDBOperationConfig {IndexName = DatabaseConstants.GSI1IndexName});
    //    var operations = await asyncSearch.GetRemainingAsync();
    //    var operation = operations.FirstOrDefault();
    //    return _mapper.Map<DomainOperation>(operation);
    //}

    //public async Task<DomainOperation> GetByCRunnerId(string crunnerId)
    //{
    //    var asyncSearch = _dbContext.QueryAsync<DBOperation>(nameof(DBOperation),
    //        new DynamoDBOperationConfig
    //        {
    //            IndexName = DatabaseConstants.GSI2IndexName,
    //            QueryFilter = {new ScanCondition("CRunnerId", ScanOperator.Equal, crunnerId)}
    //        });
    //    var operations = await asyncSearch.GetRemainingAsync();
    //    var operation = operations.FirstOrDefault();
    //    return _mapper.Map<DomainOperation>(operation);
    //}

    //public async Task<DomainOperation> GetByNWorkerIdAndOperationType(string nworkerId, OperationType operationType)
    //{
    //    var asyncSearch = _dbContext.QueryAsync<DBOperation>(nworkerId, QueryOperator.BeginsWith,
    //        new[] {DatabaseConstants.OperationIdPrefix},
    //        new DynamoDBOperationConfig
    //        {
    //            QueryFilter = new List<ScanCondition>
    //            {
    //                new(nameof(DomainOperation.OperationType), ScanOperator.Equal,
    //                    operationType.ToString())
    //            }
    //        });
    //    var operations = await asyncSearch.GetRemainingAsync();
    //    var operation = operations.FirstOrDefault();
    //    return _mapper.Map<DomainOperation>(operation);
    //}

    public async Task<string> AddOrUpdate(Domain.Entities.Check check)
    {
        var dbCheck = _mapper.Map<DataAccess.Entities.Check>(check);
        if (String.IsNullOrEmpty(dbCheck.Id))
        {
            dbCheck.Id = Guid.NewGuid().ToString("D");
        }

        await _dbContext.SaveAsync(dbCheck);

        return dbCheck.Id;
    }

    //public async Task<List<DomainOperation>> GetUnfinishedOperations()
    //{
    //    var asyncSearch = _dbContext.QueryAsync<DBOperation>(nameof(DBOperation),
    //        new DynamoDBOperationConfig
    //        {
    //            IndexName = DatabaseConstants.GSI2IndexName,
    //            QueryFilter =
    //            {
    //                new ScanCondition(nameof(DomainOperation.OperationStatus), ScanOperator.In,
    //                    OperationStatus.Pending.ToString(), OperationStatus.InProgress.ToString())
    //            }
    //        });
    //    var operations = await asyncSearch.GetRemainingAsync();
    //    return _mapper.Map<List<DomainOperation>>(operations);
    //}
}