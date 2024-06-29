using System.CommandLine;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace XsdTools.FetchRelated;

public class FetchCommand : Command, IServiceModule
{
    public FetchCommand(HandlerFactory handlerFactory)
        : base("fetch", "Given an xsd schema, fetches all linked (imported) schemas.")
    {
        FetchArgs.Declare(this);
        Handler = handlerFactory.SimpleHandler<FetchHandler, FetchArgs>((() => [this]));
    }

    public void ConfigureServices(IServiceCollection services, IConfigurationRoot config)
    {
        services.AddSingleton(new ApplicationConfig(config));
    }
}
