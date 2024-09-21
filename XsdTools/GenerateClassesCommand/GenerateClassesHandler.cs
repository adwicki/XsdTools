using System.CommandLine;
using System.CommandLine.Invocation;

using XsdTools.Services;

namespace XsdTools.GenerateClassesCommand;

// ReSharper disable once ClassNeverInstantiated.Global
public class GenerateClassesHandler : ISimpleHandler<GenerateClassesArgs>
{
    private readonly IConsole _console;
    private readonly IClassGeneratorService _classGeneratorService;
    private readonly ApplicationConfig _config;

    public GenerateClassesHandler(IConsole console, IClassGeneratorService classGeneratorService, ApplicationConfig config)
    {
        _console = console;
        _config = config;
        _classGeneratorService = classGeneratorService;
    }

    public async Task<int> RunAsync(InvocationContext context, GenerateClassesArgs args,
        CancellationToken cancellationToken = default)
    {
        string inputFolder;

        if (string.IsNullOrEmpty(args.Folder))
        {
            var folders = Directory.GetDirectories(_config.OutputDir);

            if (folders.Length == 0)
            {
                _console.WriteLine("No folders found to process.");
                return ExitCodes.Aborted;
            }

            _console.WriteLine("Available to process:");
            var counter = 1;

            foreach (string folder in folders)
            {
                _console.WriteLine($"  {counter++}. {folder}");
            }

            _console.Write("Choose a folder: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, out int index))
            {
                _console.WriteLine("Invalid input, please provide a number.");
                return ExitCodes.Aborted;
            }

            index -= 1;

            if (index < 0 || index >= folders.Length)
            {
                _console.WriteLine("Invalid input, out of range.");
                return ExitCodes.Aborted;
            }

            inputFolder = folders[index];
        }
        else
        {
            if (!Directory.Exists(args.Folder))
            {
                _console.WriteLine($"Could not find directory {args.Folder}");
                return ExitCodes.Aborted;
            }

            inputFolder = args.Folder;
        }

        await _classGeneratorService.GenerateClasses(inputFolder, cancellationToken);

        return ExitCodes.Ok;
    }
}
