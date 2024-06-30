using System.CommandLine;
using System.Diagnostics;
using System.Text;

namespace XsdTools.Services;

public class XsdExeClassGeneratorService : IClassGeneratorService
{
    private readonly IConsole _console;

    public XsdExeClassGeneratorService(IConsole console)
    {
        _console = console;
    }
    public async Task<int> GenerateClasses(string directory,
        CancellationToken cancellationToken)
    {
        var files = Directory
            .GetFiles(directory, "*.xsd")
            .Select(f => new FileInfo(f).FullName)
            .ToArray();

        if (files.Length == 0)
        {
            _console.WriteLine($"Could not find any files in {directory}");
            return ExitCodes.Aborted;
        }

        var args = BuildArgumentString(files);

        var process = new Process();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = "xsd.exe",
            Arguments = $"/c {args}",
            CreateNoWindow = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            WorkingDirectory = directory
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

    private string BuildArgumentString(string[] files)
    {
        var sb = new StringBuilder();

        foreach (var file in files[..^1])
        {
            sb.Append($"\"{file}\"").Append(' ');
        }

        sb.Append(files[^1]);
        return sb.ToString();
    }
}
