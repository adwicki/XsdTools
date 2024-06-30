using System.CommandLine;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using XsdTools.Services;

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
        services.AddTransient<IOutputDirService>(_ =>
            new OutputDirService(services.BuildServiceProvider().GetService<ApplicationConfig>()!));
    }
}
