using Amazon.Lambda.APIGatewayEvents;

namespace CheckRegistry.Commands;

public interface IProxyRequestCommand
{
    Task<APIGatewayProxyResponse> Execute(APIGatewayProxyRequest request);
}