using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.Text;

using XsdTools.Services;

namespace XsdTools.GenerateClassesCommand;

// ReSharper disable once ClassNeverInstantiated.Global
public class GenerateClassesHandler : ISimpleHandler<GenerateClassesArgs>
{
    private readonly IConsole _console;
    private readonly IClassGeneratorService _classGeneratorService;

    public GenerateClassesHandler(IConsole console, IClassGeneratorService classGeneratorService)
    {
        _console = console;
        _classGeneratorService = classGeneratorService;
    }

    public async Task<int> RunAsync(InvocationContext context, GenerateClassesArgs args,
        CancellationToken cancellationToken = default)
    {
        if (!Directory.Exists(args.Folder))
        {
            _console.WriteLine($"Could not find directory {args.Folder}");
            return ExitCodes.Aborted;
        }

        await _classGeneratorService.GenerateClasses(args.Folder, cancellationToken);

        return ExitCodes.Ok;
    }
}
