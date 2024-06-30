namespace XsdTools.Services;

public interface IOutputDirService
{
    public void InitializeOutputDir();
    public string CreateOutputFolderFromFilePath(string filePath, bool forceRecreate = false);
    public string CopyFileToOutputFolder(string outputFolderPath, string filePath);
}
