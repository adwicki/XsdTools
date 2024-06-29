using System.CommandLine;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace XsdTools.GenerateClassesCommand;

public class GenerateClassesCommand : Command, IServiceModule
{
    public GenerateClassesCommand(HandlerFactory handlerFactory)
        : base("generate-classes", "Generate classes given a folder with xsd schemas.")
    {
        GenerateClassesArgs.Declare(this);
        Handler = handlerFactory.SimpleHandler<GenerateClassesHandler, GenerateClassesArgs>((() => [this]));
    }

    public void ConfigureServices(IServiceCollection services, IConfigurationRoot config)
    {
        services.AddSingleton(new ApplicationConfig(config));
    }
}
