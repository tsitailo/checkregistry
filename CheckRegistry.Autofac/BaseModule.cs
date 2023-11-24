using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.XRay.Recorder.Handlers.AwsSdk;
using Autofac;

namespace CheckRegistry.Autofac;

public abstract class BaseModule : Module
{
    protected const string ServiceUrl = "http://localhost:4566";
    private const string ProfileName = "default";

    protected BaseModule()
    {
        if (!IsDevelopment())
        {
            //  Enable X-Ray integration for all AWS services
            AWSSDKHandler.RegisterXRayForAllServices();
        }
    }
        
    protected static bool IsDevelopment()
    {
        return Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") == "Development";
    }

    protected static AWSCredentials GetCredentials()
    {
        var credentialProfileStoreChain = new CredentialProfileStoreChain();
        credentialProfileStoreChain.TryGetAWSCredentials(ProfileName, out var credentials);
        return credentials;
    }
}