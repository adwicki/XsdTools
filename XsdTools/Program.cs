using System.CommandLine;

using XsdTools;
using XsdTools.CleanCommand;
using XsdTools.FetchRelated;
using XsdTools.Status;

// Thanks to my lord and saviour: https://github.com/lord-executor/ModularCliTemplate

var launcher = new Launcher();

var rootCommand = new RootCommand("XsdTool")
{
    new StatusCommand(launcher.HandlerFactory),
    new FetchCommand(launcher.HandlerFactory),
    new CleanCommand(launcher.HandlerFactory)
};

return await launcher.InvokeAsync(rootCommand, args);
