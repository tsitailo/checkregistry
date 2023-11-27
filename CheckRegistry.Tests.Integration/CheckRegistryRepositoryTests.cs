using Amazon.Lambda.TestUtilities;
using CheckRegistry.DataAccess.Repositories;
using CheckRegistry.Domain.Entities;
using CheckRegistry.Domain.Interfaces;
using NUnit.Framework;

namespace CheckRegistry.Tests.Integration;

/// 
/// Preconditions:
/// 1. Localstack should be installed
///    with the following components: DynamoDB
/// 
[TestFixture, Explicit]
public class CheckRegistryRepositoryTests : BaseRepositoryTests
{
    private ICheckRegistryRepository _sut;

    [SetUp]
    public void OperationRepositoryTestsSetUp()
    {
        _sut = new CheckRegistryRepository(_dynamoDbContext, _mapper, new TestLambdaLogger());
    }


    [Test]
    [Ignore("Just for inserting test data into DB")]
    public async Task Can_AddOrUpdate_Record()
    {
        var dates = GetDatesBetween(new DateTime(2023, 01, 01), new DateTime(2023, 12, 31));
        Random random = new Random(DateTime.Now.Millisecond);

        
        for (int i =0 ; i <500000; i++)
        {
            var expectedOperation = new Check
            {
                IssueDate = dates[random.Next(0, dates.Length - 1)].ToString("O"),
                Data = GenerateCheck(),
                Status = GenerateStatus(),
                RegistrationResults = new List<RegistrationResult>
                {
                    new RegistrationResult { ServiceName = "tax", Result = GeneRegistrationOutcome() },
                    new RegistrationResult { ServiceName = "logistics", Result = GeneRegistrationOutcome() }
                }

            };
             _sut.AddOrUpdate(expectedOperation);
        };
    }


}