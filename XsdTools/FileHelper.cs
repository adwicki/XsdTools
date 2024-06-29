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

    public static bool OutputDirectoryExists(string outputPath)
    {
        return Directory.Exists(GetSubDirectory(outputPath));
    }

    private static string GetSubDirectory(string directory)
    {
        return Path.Join(Directory.GetCurrentDirectory(), directory);
    }
}
