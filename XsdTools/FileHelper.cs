using System.Text;

namespace XsdTools;

public static class FileHelper
{
    public static void EnsureOutputDirectoryExists(string outputPath)
    {
        var outputDir = GetSubDirectory(outputPath);
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }
    }

    public static string CreateValidPath(string name) {
        var builder = new StringBuilder();
        var invalid = Path.GetInvalidPathChars();
        foreach (var cur in name.Where(cur => !invalid.Contains(cur)))
        {
            builder.Append(cur);
        }
        return builder.ToString();
    }

    private static string GetSubDirectory(string directory)
    {
        return Path.Join(Directory.GetCurrentDirectory(), directory);
    }

    public static string GetConcreteOutputDirectory(string configOutputDir)
    {
        return GetSubDirectory(configOutputDir);
    }
}
