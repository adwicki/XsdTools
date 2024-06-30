using System.CommandLine;

namespace XsdTools.Services;

public interface IClassGeneratorService
{
    public Task<int> GenerateClasses(string directory, CancellationToken cancellationToken);
}
