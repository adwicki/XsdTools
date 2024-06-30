using System.CommandLine;
using System.Diagnostics.CodeAnalysis;

namespace XsdTools.FetchRelated;

// ReSharper disable once ClassNeverInstantiated.Global
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class FetchArgs
{
    public required string InputXsd { get; set; }
    public required bool Clean { get; set; }

    public static void Declare(Command command)
    {
        command.AddArgument(new Argument<string>("InputXsd")
        {
            Description = "The 'root' xsd file for which related schemas should be fetched"
        });

        command.AddOption(new Option<bool>("--clean")
        {
            Description = "If set, the output directory will be deleted before generation."
        });
    }
}
