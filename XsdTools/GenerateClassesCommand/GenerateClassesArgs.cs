using System.CommandLine;

namespace XsdTools.GenerateClassesCommand;

public class GenerateClassesArgs
{
    public required string Folder { get; set; }

    public static void Declare(Command command)
    {
        command.AddArgument(new Argument<string>("Folder")
        {
            Description = "Specify the folder to process with the xsd tool."
        });
    }
}
