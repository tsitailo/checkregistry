using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.Core;
using AutoMapper;
using CheckRegistry.Domain.Interfaces;
using Newtonsoft.Json;

namespace CheckRegistry.DataAccess.Repositories;

public class CheckRegistryRepository : ICheckRegistryRepository
{
    private readonly IDynamoDBContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILambdaLogger _logger;

    public CheckRegistryRepository(IDynamoDBContext dbContext, IMapper mapper, ILambdaLogger logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string> AddOrUpdate(Domain.Entities.Check check)
    {
        _logger.LogLine($"Domain check: {JsonConvert.SerializeObject(check)}");

        var dbCheck = _mapper.Map<DataAccess.Entities.Check>(check);
        _logger.LogLine($"DB check: {JsonConvert.SerializeObject(dbCheck)}");

        if (String.IsNullOrEmpty(dbCheck.Id))
        {
            dbCheck.Id = Guid.NewGuid().ToString("D");
        }

        await _dbContext.SaveAsync(dbCheck);

        return dbCheck.Id;
    }
}