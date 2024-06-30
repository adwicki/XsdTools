using System.Text;

namespace XsdTools.Services;

public class OutputDirService : IOutputDirService
{
    private ApplicationConfig Config { get; }

    public OutputDirService(ApplicationConfig config)
    {
        Config = config;
    }

    public void InitializeOutputDir()
    {
        var outputDir = GetSubDirectory(Config.OutputDir);
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }
    }

    public string CreateOutputFolderFromFilePath(string filePath, bool forceRecreate = false)
    {
        var folderName = Path.GetFileNameWithoutExtension(filePath);
        var basePath = Path.Join(Directory.GetCurrentDirectory(), Config.OutputDir, folderName);
        basePath = CreateValidPath(basePath);

        if (Path.Exists(basePath) && forceRecreate)
        {
            Directory.Delete(basePath, true);
        }

        if (!Path.Exists(basePath))
        {
            Directory.CreateDirectory(basePath);
        }

        return basePath;
    }

    public string CopyFileToOutputFolder(string outputFolderPath, string filePath)
    {
        var newPath = Path.Join(outputFolderPath, Path.GetFileName(filePath));

        File.Copy(
            filePath,
            newPath,
            true);

        return newPath;
    }

    private static string GetSubDirectory(string directory)
    {
        return Path.Join(Directory.GetCurrentDirectory(), directory);
    }

    private static string CreateValidPath(string name) {
        var builder = new StringBuilder();
        var invalid = Path.GetInvalidPathChars();
        foreach (var cur in name.Where(cur => !invalid.Contains(cur)))
        {
            builder.Append(cur);
        }
        return builder.ToString();
    }
}
