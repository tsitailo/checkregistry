using System.Diagnostics;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using CheckRegistry.AddChecks;
using CheckRegistry.Commands;
using CheckRegistry.DataAccess.Repositories;
using CheckRegistry.Domain.Entities;
using CheckRegistry.Domain.Interfaces;
using Moq;
using NUnit.Framework;
using System.Text;


namespace CheckRegistry.Tests.Unit;

[TestFixture]
public class AddChecksCommandTests
{
    private AddChecksCommand _sut;
    private Mock<ICheckRegistryRepository> _checkRegistryRepositoryMock;
    private Mock<ILambdaLogger> _lambdaLoggerMock;
    private ILambdaContext _lambdaContext;
    private Mock<IEventSender> _eventSenderMock;


    [SetUp]
    public void SetUp()
    {
        _lambdaLoggerMock = new Mock<ILambdaLogger>();
        _lambdaContext = new TestLambdaContext { Logger = _lambdaLoggerMock.Object };
        _checkRegistryRepositoryMock = new Mock<ICheckRegistryRepository>();
        _eventSenderMock = new Mock<IEventSender>();

        _sut = new AddChecksCommand(_checkRegistryRepositoryMock.Object, new ResponseBuilder(), _eventSenderMock.Object, _lambdaContext);
    }

    [Test]
    public async Task Can_Add_Checks()
    {
        var expectedStatusCode = 200;

        _lambdaLoggerMock.Setup(_ => _.LogLine(It.IsAny<string>()));
        _checkRegistryRepositoryMock.Setup(_ => _.AddOrUpdate(It.IsAny<Check>())).ReturnsAsync((Check check) =>
        {
            return check.Id;
        });

        var base64EncodedCheck = "e1wic2hvcElkXCI6IFwiMVwiLCBcIml0ZW1zXCI6IFt7XCJuYW1lXCI6XCJndW5cIiwgXCJhbW91bnRcIjpcIjJcIiwgXCJwcmljZVwiOlwiMTBcIn0sIHtcIm5hbWVcIjpcImNhclwiLCBcImFtb3VudFwiOlwiMVwiLCBcInByaWNlXCI6XCIxMDAwXCJ9XX0=";

        var body = "{\"checks\": [\"" + base64EncodedCheck + "\", \"" + base64EncodedCheck + "\"]}";

        var apiGatewayProxyRequest = new APIGatewayProxyRequest
        {

            Headers = new Dictionary<string, string> { { "Host", "host.com" } },
            RequestContext = new APIGatewayProxyRequest.ProxyRequestContext { Path = "/path" },
            Body = "{\"data\":\"" + EncodeBase64(body) + "\"}"
            //Data = "{\"checks\": [\"{\"shopId\": \"1\", \"items\": [{\"name\":\"gun\", \"amount\":\"2\", \"price\":\"10\"}, {\"name\":\"car\", \"amount\":\"1\", \"price\":\"1000\"}]}\", \"{\"shopId\": \"1\", \"items\": [{\"name\":\"gun\", \"amount\":\"3\", \"price\":\"10\"}, {\"name\":\"car\", \"amount\":\"21\", \"price\":\"1000\"}]}\"]}"
        };

        Debug.WriteLine(apiGatewayProxyRequest.Body);

        var response = await _sut.Execute(apiGatewayProxyRequest);

        Assert.NotNull(response);
        Assert.AreEqual(expectedStatusCode, response.StatusCode);
        Assert.NotNull(response.Body);
        
        _eventSenderMock.Verify(_ => _.Send(It.IsAny<BaseEvent>(), It.IsAny<ILambdaContext>()), Times.AtLeastOnce);

    }

    private  string EncodeBase64( string value)
    {
        var valueBytes = Encoding.UTF8.GetBytes(value);
        return Convert.ToBase64String(valueBytes);
    }
}