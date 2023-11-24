using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.XRay.Recorder.Handlers.AwsSdk;
using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using CheckRegistry.Autofac;
using CheckRegistry.DataAccess.Repositories;
using CheckRegistry.Domain.Interfaces;

namespace CheckRegistry.DataAccess;

public class DataAccessModule : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(_ =>
                IsDevelopment()
                    ? new AmazonDynamoDBClient(GetCredentials(), new AmazonDynamoDBConfig {ServiceURL = ServiceUrl})
                    : new AmazonDynamoDBClient())
            .As<IAmazonDynamoDB>();
        builder.RegisterType<DynamoDBContext>().As<IDynamoDBContext>();
        
        builder.RegisterType<CheckRegistryRepository>().As<ICheckRegistryRepository>();
        builder.RegisterAutoMapper(typeof(DataAccessModule).Assembly);
    }
}