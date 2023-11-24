using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Autofac;
using CheckRegistry.Autofac;
using CheckRegistry.Commands;

namespace CheckRegistry.AddChecks
{
    public  class AddCheckHandler
    {
        private readonly IContainer _container;

        public AddCheckHandler() : this(new AddChecksContainerConfigurator())
        {
        }

        public AddCheckHandler(IContainerConfigurator containerConfigurator)
        {
            _container = containerConfigurator.Configure();
        }

        public async Task<APIGatewayProxyResponse> Handle(APIGatewayProxyRequest request, ILambdaContext context)
        {
            await using var scope = _container.BeginLifetimeScope();
            return await scope.Resolve<IProxyRequestCommand>(new NamedParameter(nameof(context), context)).Execute(request);
        }

    }
}
