using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace XsdTools;

public interface IServiceModule
{
    void ConfigureServices(IServiceCollection services, IConfigurationRoot config);
}
