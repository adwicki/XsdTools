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
        string mainFileName = new DirectoryInfo(directory).Name;
        var files = new List<string>();
        var mainFile = Directory.GetFiles(directory, $"{mainFileName}.xsd")
            .Select(f => new FileInfo(f).FullName)
            .FirstOrDefault();

        if (mainFile == null)
        {
            _console.WriteLine($"Couldn't find main-file {mainFileName}");
            return ExitCodes.Aborted;
        }

        files.Add(mainFile);

        var relatedFiles = Directory
            .GetFiles(directory, "*.xsd")
            .Select(f => new FileInfo(f).FullName)
            .Where(f => f != mainFileName)
            .ToList();

        files.AddRange(relatedFiles);

        var args = BuildArgumentString(files.ToArray());

        _console.WriteLine($"Will run:\nxsd.exe /c {args}");

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

        sb.Append($"\"{files[^1]}\"");
        return sb.ToString();
    }
}
