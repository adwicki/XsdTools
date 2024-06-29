using System.CommandLine;
using System.CommandLine.Invocation;
using System.Xml;

namespace XsdTools.FetchRelated;

// ReSharper disable once ClassNeverInstantiated.Global
public class FetchHandler : ISimpleHandler<FetchArgs>
{
    private readonly IConsole _console;
    private readonly ApplicationConfig _config;

    public FetchHandler(IConsole console, ApplicationConfig config)
    {
        _console = console;
        _config = config;
    }

    public async Task<int> RunAsync(InvocationContext context, FetchArgs args,
        CancellationToken cancellationToken = default)
    {
        FileHelper.EnsureOutputDirectoryExists(_config.OutputDir);

        var xsd = new XmlDocument();
        xsd.LoadXml(await File.ReadAllTextAsync(args.InputXsd, cancellationToken));

        return ExitCodes.Ok;
    }
}
