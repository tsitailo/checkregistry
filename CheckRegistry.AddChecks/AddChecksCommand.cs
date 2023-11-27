using System.Net;
using System.Text;
using Amazon.EventBridge.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Runtime.Internal;
using CheckRegistry.Commands;
using CheckRegistry.DataAccess.Entities;
using CheckRegistry.Domain.Events;
using CheckRegistry.Domain.Interfaces;
using CheckRegistry.Domain.Tools;
using Microsoft.Extensions.Logging.EventLog;
using Newtonsoft.Json;
using Check = CheckRegistry.Domain.Entities.Check;
using RegistrationResult = CheckRegistry.Domain.Entities.RegistrationResult;

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
                _context.Logger.LogLine(request.Body);

                var envelop = JsonConvert.DeserializeObject<CheckEnvelop>(request.Body);

                if (envelop != null)
                {
                    var data = DecodeBase64(envelop.Data); 

                    var checks = JsonConvert.DeserializeObject<CheckData>(data);

                    _context.Logger.LogLine($"Checks count : {checks?.Checks.Count}");

                    var messages = new List<Task>();

                    if (checks != null)
                    {
                        foreach (var checkData in checks.Checks)
                        {
                            Check check = new Check
                            {
                                Id = Guid.NewGuid().ToString("D"),
                                Data = checkData,
                                Status = "PENDING",
                                IssueDate = DateTime.Now.ToString("O"),
                                RegistrationResults = new List<RegistrationResult>(),
                            };
                            string id = await InsertCheckIntoDb(check);
                            results.Add(id);

                            //todo: VerifyCheck();

                            messages.Add(SendRegisterCheckCommand(check));
                        }
                    }

                    Task.WaitAll(messages.ToArray());
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
        var @event = new RegisterCheckEvent
        {
            ServiceName = "CheckRegistry",
            CheckData = check.Data,
            CheckId = check.Id
        };

        await _eventSender.Send(@event, _context);
    }


    private async Task<string> InsertCheckIntoDb(Check check)
    {
        _context.Logger.LogLine(JsonConvert.SerializeObject(check));
        try
        {
            return await _checkRepository.AddOrUpdate(check);
        }
        catch (Exception e)
        {
            _context.Logger.LogLine(e.Message);
            _context.Logger.LogLine(e.StackTrace);
            throw;
        }

    }

    private string DecodeBase64(string value)
    {
        var valueBytes = System.Convert.FromBase64String(value);
        return Encoding.UTF8.GetString(valueBytes);
    }
}