using System.CommandLine;

using XsdTools;
using XsdTools.FetchRelated;
using XsdTools.GenerateClassesCommand;

// Thanks to my lord and saviour: https://github.com/lord-executor/ModularCliTemplate

var launcher = new Launcher();

var rootCommand = new RootCommand("XsdTool")
{
    new FetchCommand(launcher.HandlerFactory),
    new GenerateClassesCommand(launcher.HandlerFactory)
};

return await launcher.InvokeAsync(rootCommand, args);
