using Amazon.EventBridge;
using Amazon.EventBridge.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using CheckRegistry.Domain.Interfaces;
using CheckRegistry.Domain.Tools;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CheckRegistry.Tests.Unit;

[TestFixture]
public class EventSenderTests
{
    private IEventSender _sut;
    private Mock<IAmazonEventBridge> _amazonEventBridgeMock;
    private Mock<ILambdaLogger> _lambdaLoggerMock;
    private ILambdaContext _lambdaContext;

    [SetUp]
    public void SetUp()
    {
        _amazonEventBridgeMock = new Mock<IAmazonEventBridge>();
        _lambdaLoggerMock = new Mock<ILambdaLogger>();
        _lambdaContext = new TestLambdaContext {Logger = _lambdaLoggerMock.Object};

        _sut = new EventSender(_amazonEventBridgeMock.Object);
    }

    [Test]
    public async Task Can_Send_Events()
    {
        _lambdaLoggerMock.Setup(_ => _.LogLine(It.IsAny<string>()));
        _amazonEventBridgeMock.Setup(_ =>
            _.PutEventsAsync(It.IsAny<PutEventsRequest>(), It.IsAny<CancellationToken>()));

        var expectedEvent = new Domain.Events.RegisterCheckEvent();

        await _sut.Send(expectedEvent, _lambdaContext);

        _amazonEventBridgeMock.Verify(
            _ => _.PutEventsAsync(It.IsAny<PutEventsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}