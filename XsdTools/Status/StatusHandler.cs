using System.CommandLine;
using System.CommandLine.Invocation;

namespace XsdTools.Status;

// ReSharper disable once ClassNeverInstantiated.Global
public class StatusHandler : ISimpleHandler<StatusArgs>
{
    private readonly IConsole _console;
    private readonly ApplicationConfig _config;

    public StatusHandler(IConsole console, ApplicationConfig config)
    {
        _console = console;
        _config = config;
        _console.WriteLine("StatusHandler created with console");
    }

    public async Task<int> RunAsync(InvocationContext context, StatusArgs args, CancellationToken cancellationToken = default)
    {
        _console.WriteLine("Hello StatusHandler!");
        await Task.Delay(_config.Delay, cancellationToken);
        _console.WriteLine("Done");

        return ExitCodes.Ok;
    }
}
