using Autofac;
using CheckRegistry.Autofac;
using CheckRegistry.Domain.Interfaces;

namespace CheckRegistry.ProxyLambdaLogger;

public class LambdaLoggerModule : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<Logger>().As<ILogger>();
    }
}