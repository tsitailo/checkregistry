using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Runtime.Internal;
using CheckRegistry.Commands;
using CheckRegistry.DataAccess.Entities;
using CheckRegistry.Domain.Events;
using CheckRegistry.Domain.Interfaces;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using Check = CheckRegistry.Domain.Entities.Check;

namespace CheckRegistry.AddChecks;

public class AddChecksCommand : IProxyRequestCommand
{
    private readonly ICheckRegistryRepository _checkRepository;
    private readonly ResponseBuilder _responseBuilder;
    private readonly ILambdaContext _context;
    private readonly IEventSender _eventSender;


    public AddChecksCommand(ICheckRegistryRepository checkRepository, ResponseBuilder responseBuilder, IEventSender eventSender, ILambdaContext context)
    {
        _checkRepository = checkRepository;
        _responseBuilder = responseBuilder;
        _eventSender = eventSender;
        _context = context;
    }

    public async Task<APIGatewayProxyResponse> Execute(APIGatewayProxyRequest request)
    {
        try
        {
            var results = new List<string>();
            try
            {
                var checks = JsonConvert.DeserializeObject<CheckData>(request.Body);
                if (checks != null)
                {
                    foreach (var checkData in checks.Checks)
                    {
                        Check check = new Check
                        {
                            Id = Guid.NewGuid().ToString("D"),
                            Data = checkData
                        };
                        string id = await InsertCheckIntoDb(check);
                        results.Add(id);

                        //todo: VerifyCheck();

                        await SendRegisterCheckCommand(check);
                    }
                }
            }
            catch (Exception ex)
            {
                //todo: return exception
            }
 
            var responseBody = new AddChecksResponse();
            responseBody.Checks.AddRange(results);

            return _responseBuilder.WithBody(responseBody).WithStatusCode((int)HttpStatusCode.OK).Build();
        }
        catch (Exception exception)
        {
            _context.Logger.LogLine(exception.ToString());
            return _responseBuilder
                .WithBody(new ErrorResponse { Message = exception.Message })
                .WithStatusCode((int)HttpStatusCode.InternalServerError)
                .Build();
        }
    }

    private async Task SendRegisterCheckCommand(Check check)
    {
        var @event = new RegisterCheckEvent()
        {
            ServiceName = "",
            CheckData = check.Data,
            CheckId = check.Id
        };
        await _eventSender.Send(@event, _context);
    }


    private async Task<string> InsertCheckIntoDb(Check check)
    {
        return await _checkRepository.AddOrUpdate(check);
    }
}