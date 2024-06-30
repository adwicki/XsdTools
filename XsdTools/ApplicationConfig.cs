using Microsoft.Extensions.Configuration;

namespace XsdTools;

public class ApplicationConfig
{
    public string OutputDir { get; }

    public ApplicationConfig(IConfiguration config)
    {
        OutputDir = config.GetValue<string>("OutputDir") ?? string.Empty;
    }
}
