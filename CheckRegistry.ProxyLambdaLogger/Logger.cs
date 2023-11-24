using CheckRegistry.Domain.Interfaces;
using Amazon.Lambda.Core;

namespace CheckRegistry.ProxyLambdaLogger;

public class Logger : ILogger
{
    public void LogLine(string message)
    {
        LambdaLogger.Log(message);
    }
}