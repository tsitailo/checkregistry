using Autofac;

namespace CheckRegistry.Autofac;

public interface IContainerConfigurator
{
    IContainer Configure();
}