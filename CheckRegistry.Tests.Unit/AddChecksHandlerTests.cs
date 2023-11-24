using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Autofac;
using CheckRegistry.AddChecks;
using CheckRegistry.Autofac;
using CheckRegistry.Commands;
using CheckRegistry.DataAccess;
using Moq;
using NUnit.Framework;

namespace CheckRegistry.Tests.Unit;

[TestFixture]
public class AddChecksHandlerTests
{
    private AddCheckHandler _sut;
    private Mock<IContainerConfigurator> _containerConfiguratorMockMock;
    private Mock<ILambdaLogger> _lambdaLoggerMock;
    private ILambdaContext _lambdaContext;

    [SetUp]
    public void SetUp()
    {
        _lambdaLoggerMock = new Mock<ILambdaLogger>();
        _lambdaContext = new TestLambdaContext {Logger = _lambdaLoggerMock.Object};
        _containerConfiguratorMockMock = new Mock<IContainerConfigurator>();
        _containerConfiguratorMockMock.Setup(_ => _.Configure()).Returns(BuildTestContainer);
        _sut = new AddCheckHandler(_containerConfiguratorMockMock.Object);
    }

    [Test]
    public async Task Can_Handle_Request()
    {
        var response = await _sut.Handle(new APIGatewayProxyRequest(), _lambdaContext);

        Assert.NotNull(response);
        Assert.IsInstanceOf<APIGatewayProxyResponse>(response);
    }

    private static IContainer BuildTestContainer()
    {
        var getNWorkersCommandMock = new Mock<IProxyRequestCommand>();
        getNWorkersCommandMock.Setup(_ => _.Execute(It.IsAny<APIGatewayProxyRequest>()))
            .ReturnsAsync(new APIGatewayProxyResponse());

        var builder = new ContainerBuilder();
        builder.RegisterModule<DataAccessModule>();
        builder.RegisterInstance(getNWorkersCommandMock.Object).As<IProxyRequestCommand>();
        return builder.Build();
    }
}