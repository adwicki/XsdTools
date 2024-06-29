using Microsoft.Extensions.Configuration;

namespace XsdTools;

public class ApplicationConfig
{
    public int Delay { get; }
    public string OutputDir { get; }

    public ApplicationConfig(IConfiguration config)
    {
        Delay = config.GetValue<int>("Delay");
        OutputDir = config.GetValue<string>("OutputDir") ?? string.Empty;
    }
}
