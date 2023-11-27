using Amazon.Lambda.Core;
using Autofac;

namespace CheckRegistry.Autofac;

public interface IContainerConfigurator
{
    ContainerBuilder Configure();
    ContainerBuilder Configure(ILambdaLogger logger);
}