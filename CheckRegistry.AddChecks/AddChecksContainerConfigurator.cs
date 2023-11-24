using Amazon.EventBridge;
using Amazon.XRay.Recorder.Handlers.AwsSdk;
using Autofac;
using CheckRegistry.Autofac;
using CheckRegistry.Commands;
using CheckRegistry.DataAccess;
using CheckRegistry.Domain.Interfaces;
using CheckRegistry.Domain.Tools;
using CheckRegistry.ProxyLambdaLogger;

namespace CheckRegistry.AddChecks;

public class AddChecksContainerConfigurator : BaseModule, IContainerConfigurator
{
    public IContainer Configure()
    {
        var builder = new ContainerBuilder();
        builder.RegisterModule<DataAccessModule>();
        builder.RegisterModule<LambdaLoggerModule>();
        builder.RegisterType<AddChecksCommand>().AsImplementedInterfaces();
        builder.RegisterType<ResponseBuilder>().AsSelf();

        builder.Register(_ =>
                IsDevelopment()
                    ? new AmazonEventBridgeClient(GetCredentials(),
                        new AmazonEventBridgeConfig { ServiceURL = ServiceUrl })
                    : new AmazonEventBridgeClient())
            .As<IAmazonEventBridge>();
        builder.RegisterType<EventSender>().As<IEventSender>();
        AWSSDKHandler.RegisterXRay<IAmazonEventBridge>();

        return builder.Build();
    }
}