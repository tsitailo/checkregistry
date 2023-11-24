using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;

namespace CheckRegistry.Commands;

public class ResponseBuilder
{
    private readonly APIGatewayProxyResponse _response;

    public ResponseBuilder()
    {
        _response = new APIGatewayProxyResponse
        {
            Headers = new Dictionary<string, string> {{"Content-Type", "application/json"}}
        };
    }

    public ResponseBuilder WithBody(object body)
    {
        _response.Body = JsonConvert.SerializeObject(body);
        return this;
    }

    public ResponseBuilder WithStatusCode(int statusCode)
    {
        _response.StatusCode = statusCode;
        return this;
    }

    public APIGatewayProxyResponse Build()
    {
        return _response;
    }
}