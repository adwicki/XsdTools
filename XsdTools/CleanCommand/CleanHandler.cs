using System.CommandLine;
using System.CommandLine.Invocation;

namespace XsdTools.CleanCommand;

public class CleanHandler : ISimpleHandler<NoArgs>
{
    private readonly IConsole _console;
    private readonly ApplicationConfig _config;

    public CleanHandler(IConsole console, ApplicationConfig config)
    {
        _console = console;
        _config = config;
    }

    public Task<int> RunAsync(InvocationContext context, NoArgs args, CancellationToken cancellationToken = default)
    {
        if (FileHelper.OutputDirectoryExists(_config.OutputDir))
        {
            _console.WriteLine($"Cleaning directory {_config.OutputDir}");
            var fileCount = Directory.EnumerateFiles(_config.OutputDir).Count();
            _console.Write($"Found {fileCount} files...");
            Directory.Delete(_config.OutputDir);
            _console.WriteLine($" deleted all of them.");
        }
        else
        {
            _console.WriteLine("OutputDir doesn't exist → skipping.");
        }

        return Task.FromResult(ExitCodes.Ok);
    }
}
