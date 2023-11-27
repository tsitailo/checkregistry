using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Autofac;
using CheckRegistry.Autofac;
using CheckRegistry.Commands;
using CheckRegistry.Domain.Interfaces;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace CheckRegistry.AddChecks
{
    public  class AddCheckHandler
    {
        private readonly IContainerConfigurator _configurator;

        public AddCheckHandler() : this(new AddChecksContainerConfigurator())
        {
            
        }

        public AddCheckHandler(IContainerConfigurator containerConfigurator)
        {
            _configurator = containerConfigurator;
        }

        public async Task<APIGatewayProxyResponse> Handle(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var builder = _configurator.Configure(context.Logger);
            builder.RegisterInstance(context).As<ILambdaContext>();
            var container = builder.Build();

            await using var scope = container.BeginLifetimeScope();

            var repository = scope.Resolve<ICheckRegistryRepository>(new NamedParameter("logger", context.Logger));
            return await scope.Resolve<IProxyRequestCommand>(new NamedParameter(nameof(context), context), new NamedParameter("checkRepository", repository)).Execute(request);
        }

    }
}
