using System.CommandLine;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace XsdTools.CleanCommand;

public class CleanCommand : Command, IServiceModule
{
    public CleanCommand(HandlerFactory handlerFactory)
        : base("clean", "Cleans the output directory, i.e. deletes it.")
    {
        Handler = handlerFactory.SimpleHandler<CleanHandler, NoArgs> (() => [this]);
    }

    public void ConfigureServices(IServiceCollection services, IConfigurationRoot config)
    {
        services.AddSingleton(new ApplicationConfig(config));
    }
}
