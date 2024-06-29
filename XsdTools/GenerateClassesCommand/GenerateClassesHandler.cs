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

        var files = Directory.EnumerateFiles(args.Folder, "xsd").ToArray();

        if (files.Length == 0)
        {
            _console.WriteLine($"Could not find any files in {args.Folder}");
            return ExitCodes.Aborted;
        }

        var sb = new StringBuilder();
        sb.Append("xsd /c ");

        foreach (var file in files[..^2])
        {
            sb.Append(file).Append(' ');
        }

        sb.Append(files[^1]);

        var process = new Process();
        process.StartInfo = new ProcessStartInfo()
        {
            FileName = sb.ToString(),
            CreateNoWindow = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false
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
