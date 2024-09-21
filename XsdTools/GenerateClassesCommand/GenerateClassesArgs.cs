using System.CommandLine;

namespace XsdTools.GenerateClassesCommand;

// ReSharper disable once ClassNeverInstantiated.Global
public class GenerateClassesArgs
{
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public required string Folder { get; set; }

    public static void Declare(Command command)
    {
        command.AddArgument(new Argument<string>(
            "Folder",
            () => String.Empty,
            "Specify the folder to process with the xsd tool."));
    }
}
