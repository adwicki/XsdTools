﻿using System.CommandLine;
using System.CommandLine.Invocation;
using System.Xml;

using XsdTools.Services;

namespace XsdTools.FetchRelated;

// ReSharper disable once ClassNeverInstantiated.Global
public class FetchHandler : ISimpleHandler<FetchArgs>, IDisposable
{
    private readonly IConsole _console;
    private readonly ApplicationConfig _config;
    private readonly HttpClient _client;
    private readonly IOutputDirService _outputDirService;

    public FetchHandler(IConsole console, ApplicationConfig config, IOutputDirService outputDirService)
    {
        _console = console;
        _config = config;
        _outputDirService = outputDirService;
        _client = new HttpClient();
    }

    public async Task<int> RunAsync(InvocationContext context, FetchArgs args,
        CancellationToken cancellationToken = default)
    {
        _outputDirService.InitializeOutputDir();

        if (!File.Exists(args.InputXsd))
        {
            _console.WriteLine($"Could not find file {args.InputXsd}.");
            return ExitCodes.Aborted;
        }

        var basePath = _outputDirService.CreateOutputFolderFromFilePath(args.InputXsd, args.Clean);
        _console.WriteLine($"Created new output directory: {basePath}");

        var newPath = _outputDirService.CopyFileToOutputFolder(basePath, args.InputXsd);
        _console.WriteLine("Copied base file, now starting processing...");

        return await ProcessDocument(newPath, basePath, 0, cancellationToken);
    }

    private static string GetSpaces(int level)
    {
        return new string(' ', level * 2);
    }

    private async Task<int> ProcessDocument(string xsdPath, string basePath, int level, CancellationToken cancellationToken)
    {
        var xsd = new XmlDocument();
        try
        {
            xsd.LoadXml(await File.ReadAllTextAsync(xsdPath, cancellationToken));
        }
        catch (FileNotFoundException)
        {
            _console.WriteLine($"{GetSpaces(level)}Could not find file {xsdPath}.");
            return ExitCodes.Aborted;
        }

        var importNodes = xsd.SelectNodes("//*[local-name()='import']");

        if (importNodes is { Count: > 0 })
        {
            _console.WriteLine($"{GetSpaces(level)}Found {importNodes.Count} imports in {Path.GetFileNameWithoutExtension(xsdPath)}.");

            foreach (var importNode in importNodes.Cast<XmlElement>())
            {
                var schemaLocation = importNode.GetAttribute("schemaLocation");
                _console.WriteLine($"{GetSpaces(level)} - Fetching {schemaLocation}...");
                string fileName = await FetchSchema(schemaLocation, basePath, level);

                await ProcessDocument(fileName, basePath, level + 1, cancellationToken);

                _console.WriteLine($"{GetSpaces(level)} - Rewriting schema locations for {Path.GetFileName(xsdPath)}");
                RewriteImportNodeSchemaLocation(importNode, fileName, level);
            }

            xsd.Save(xsdPath);
        }
        else
        {
            _console.WriteLine($"{GetSpaces(level)}Found no import nodes in {Path.GetFileNameWithoutExtension(xsdPath)}, stopping exploration.");
            return ExitCodes.Ok;
        }

        return ExitCodes.Ok;
    }

    private void RewriteImportNodeSchemaLocation(XmlElement importNode, string newLocation, int level)
    {
        var oldPath = importNode.GetAttribute("schemaLocation");
        var newPath = $"./{Path.GetFileName(newLocation)}";

        importNode.Attributes.RemoveNamedItem("schemaLocation");
        importNode.SetAttribute("schemaLocation", null, newPath);

        _console.WriteLine($"{GetSpaces(level)} - From {oldPath} → {newPath}");
    }

    private async Task<string> FetchSchema(string url, string basePath, int level)
    {
        var lastSlashIndex = url.LastIndexOf('/') + 1;
        var filePath =  Path.Join(basePath, url.Substring(lastSlashIndex, url.Length - lastSlashIndex));

        if (File.Exists(filePath))
        {
            return filePath;
        }

        _console.Write($"{GetSpaces(level)} - Downloading schema for {Path.GetFileNameWithoutExtension(filePath)}...");

        var response = await _client.GetAsync(url);
        await using var resultStream = await response.Content.ReadAsStreamAsync();
        await using var fileStream = File.Create(filePath);
        await resultStream.CopyToAsync(fileStream);

        _console.WriteLine(" done!");

        return filePath;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _client.Dispose();
    }
}
