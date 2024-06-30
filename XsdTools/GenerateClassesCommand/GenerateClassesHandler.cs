using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.Text;

namespace XsdTools.GenerateClassesCommand;

// ReSharper disable once ClassNeverInstantiated.Global
public class GenerateClassesHandler : ISimpleHandler<GenerateClassesArgs>
{
    private readonly IConsole _console;
    private readonly ApplicationConfig _config;

    public GenerateClassesHandler(IConsole console, ApplicationConfig config)
    {
        _console = console;
        _config = config;
    }

    public async Task<int> RunAsync(InvocationContext context, GenerateClassesArgs args,
        CancellationToken cancellationToken = default)
    {
        if (!Directory.Exists(args.Folder))
        {
            _console.WriteLine($"Could not find directory {args.Folder}");
            return ExitCodes.Aborted;
        }

        var files = Directory
            .GetFiles(args.Folder, "*.xsd")
            .Select(f => new FileInfo(f).FullName)
            .ToArray();

        if (files.Length == 0)
        {
            _console.WriteLine($"Could not find any files in {args.Folder}");
            return ExitCodes.Aborted;
        }

        var sb = new StringBuilder();

        foreach (var file in files[..^1])
        {
            sb.Append($"\"{file}\"").Append(' ');
        }

        sb.Append(files[^1]);

        _console.WriteLine($"Full args: \n {sb}");

        var process = new Process();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = "xsd.exe",
            Arguments = $"/c {sb}",
            CreateNoWindow = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            WorkingDirectory = args.Folder
        };
        process.Start();

        var stdError = await process.StandardError.ReadToEndAsync(cancellationToken);
        var stdOut = await process.StandardOutput.ReadToEndAsync(cancellationToken);

        await process.WaitForExitAsync(cancellationToken);

        if (stdError.Length > 0)
        {
            _console.WriteLine($"Error: {stdError}");
        }

        if (stdOut.Length > 0)
        {
            _console.WriteLine(stdOut);
        }

        return ExitCodes.Ok;
    }
}
