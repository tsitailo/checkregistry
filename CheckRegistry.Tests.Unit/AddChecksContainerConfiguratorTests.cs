using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Autofac;
using CheckRegistry.AddChecks;
using CheckRegistry.Autofac;
using CheckRegistry.Commands;
using Moq;
using NUnit.Framework;

namespace CheckRegistry.Tests.Unit;

[TestFixture]
public class AddChecksContainerConfiguratorTests
{
    private IContainerConfigurator _sut;
    private ILambdaContext _lambdaContext;
    private Mock<ILambdaLogger> _lambdaLoggerMock;

    [SetUp]
    public void SetUp()
    {
        _sut = new AddChecksContainerConfigurator();
        _lambdaLoggerMock = new Mock<ILambdaLogger>();
        _lambdaContext = new TestLambdaContext { Logger = _lambdaLoggerMock.Object };
    }

    [Test]
    public void Can_Configure_Container()
    {
        var container = _sut.Configure().Build();

        Assert.NotNull(container);
        Assert.IsInstanceOf<IContainer>(container);
        Assert.True(container.IsRegistered<IProxyRequestCommand>());
    }

    [Test]
    public void Can_Resolve_AddChecks_Command()
    {
        var container = _sut.Configure(_lambdaLoggerMock.Object).Build();
        var scope = container.BeginLifetimeScope();
        var command = scope.Resolve<IProxyRequestCommand>(new NamedParameter("context", _lambdaContext));
        Assert.NotNull(command);
    }
}