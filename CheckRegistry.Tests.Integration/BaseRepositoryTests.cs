using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using AutoMapper;
using CheckRegistry.DataAccess;
using CheckRegistry.DataAccess.Mappings;
using CheckRegistry.Domain.Enums;
using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace CheckRegistry.Tests.Integration;

public class BaseRepositoryTests
{
    protected const string ServiceUrl = "http://dynamodb.us-east-1.amazonaws.com";//  "http://localhost:4566";
     protected const string ProfileName = "default";

    private IAmazonDynamoDB _dynamoDb;
    protected DynamoDBContext _dynamoDbContext;
    protected IMapper _mapper;

    [SetUp]
    public async Task SetUp()
    {
#pragma warning disable 618
        _dynamoDb = new AmazonDynamoDBClient(new StoredProfileAWSCredentials(ProfileName),
#pragma warning restore 618
            new AmazonDynamoDBConfig { ServiceURL = ServiceUrl });
        _dynamoDbContext = new DynamoDBContext(_dynamoDb);

        var mapperConfiguration = new MapperConfiguration(_ => _.AddProfile<DataAccessProfile>());
        _mapper = mapperConfiguration.CreateMapper();

        //await SetUpDynamoDB();
    }

    [TearDown]
    public async Task TearDown()
    {
        //await ClearDynamoDB();
    }

    private async Task SetUpDynamoDB()
    {
        //TODO: create table on localstack
    }

    private async Task ClearDynamoDB()
    {
        var listTablesResponse = await _dynamoDb.ListTablesAsync();
        if (listTablesResponse.TableNames.Any(_ => _ == DatabaseConstants.DynamoDBTableName))
            await _dynamoDb.DeleteTableAsync(
                new DeleteTableRequest { TableName = DatabaseConstants.DynamoDBTableName }, CancellationToken.None);
        _dynamoDbContext.Dispose();
    }

    public DateTime[] GetDatesBetween(DateTime startDate, DateTime endDate)
    {
        List<DateTime> allDates = new List<DateTime>();
        for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            allDates.Add(date);
        return allDates.ToArray();
    }

    public string GenerateCheck()
    {
        Random rnd = new Random(DateTime.Now.Millisecond);

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, 2048)
            .Select(s => s[rnd.Next(s.Length)]).ToArray());
    }

    public string GenerateStatus()
    {
        return DateTime.Now.Millisecond % 2 == 0 ? "Successful" : "Failed";
    }

    public RegistrationOutcome GeneRegistrationOutcome()
    {
        return DateTime.Now.Millisecond % 2 == 0 ? RegistrationOutcome.Success : RegistrationOutcome.Failure;
    }
}