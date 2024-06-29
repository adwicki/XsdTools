using System.CommandLine;

namespace XsdTools.FetchRelated;

public class FetchArgs
{
    public required string InputXsd { get; set; }

    public static void Declare(Command command)
    {
        command.AddArgument(new Argument<string>("InputXsd")
        {
            Description = "The 'root' xsd file for which related schemas should be fetched"
        });
    }
}
